using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Model;
using Cares.Fids.Monitor.Models.Utils;
using MonitorServerConsole.Models;

namespace MonitorServerConsole.Utils.Sockets
{
    /// <summary>
    /// 客户端扩展
    /// </summary>
    public class ClientEx
    {
        #region Private Member

        /// <summary>
        /// 代表客户端的连接
        /// </summary>
        private Socket socket;

        /// <summary>
        /// 该客户端所属的池
        /// </summary>
        private readonly ClientPool pool;

        /// <summary>
        /// 是否需要从连接池中移除
        /// </summary>
        private volatile bool isNeedToRemove;

        /// <summary>
        /// 客户端名
        /// </summary>
        private readonly String clientName;

        /// <summary>
        /// 客户端ip
        /// </summary>
        private readonly string clientIp;

        /// <summary>
        /// 客户端端口
        /// </summary>
        private readonly int clientPort;
        /// <summary>
        /// 上一次接收的时间
        /// </summary>
        private DateTime lastReceiveDateTime;


        /// <summary>
        /// 保存上一次的队列
        /// </summary>
        private List<byte> queue;

        /// <summary>
        /// 接收连续出错的次数
        /// </summary>
        private int errorTime;
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，代表一个与客户端的连接
        /// </summary>
        /// <param name="clientPool"></param>
        /// <param name="socket"></param>
        /// <param name="clientName"></param>
        public ClientEx(ClientPool clientPool,Socket socket,Tuple<string,int> clientName)
        {
            this.socket = socket;
            this.clientName = clientName==null?string.Empty:clientName.Item1+":"+clientName.Item2;
            clientIp = clientName == null ? string.Empty : clientName.Item1;
            clientPort = clientName == null ? 0 : clientName.Item2;
            this.pool = clientPool;
            lastReceiveDateTime = DateTime.Now;
            queue = new List<byte>();
            //设置发送接收超时时间
            this.socket.ReceiveTimeout = Constants.RECEIVETIMEOUT;
            this.socket.SendTimeout = Constants.SENDTIMEOUT;
        }
        #endregion

        #region Public Member

        /// <summary>
        /// 获取底层socket
        /// </summary>
        /// <returns></returns>
        public Socket GetSocket()
        {
            return socket;
        }

        /// <summary>
        /// 获取客户端名称
        /// </summary>
        /// <returns></returns>
        public String GetClientName()
        {
            return clientName;
        }

        /// <summary>
        /// 获取客户端所在的连接池
        /// </summary>
        /// <returns></returns>
        public ClientPool GetClientPool()
        {
            return pool;
        }

        /// <summary>
        /// 是否需要移除
        /// </summary>
        public bool IsNeedToRemove
        {
            get
            {
                return isNeedToRemove;
            }
        }

        /// <summary>
        /// 获取最近一次访问时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetLastVisitTime()
        {
            return lastReceiveDateTime;
        }

        /// <summary>
        /// 关闭与客户端连接
        /// </summary>
        public void ShutDown()
        {
            if (socket == null)
            {
                return;
            }
            try
            {
                //This ensures that all data is sent and received on the connected socket before it is closed.
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                LogUtils.Error(String.Format("关闭客户端连接{0} 发生异常:{1}",clientName,ex));
            }
            finally
            {
                socket.Close();
            }
        }

        #region Select
        
        /// <summary>
        /// 检查socket是否有连接
        /// </summary>
        public void Select()
        {
            try
            {
                if (isNeedToRemove)
                {
                    return;
                }

                byte[] buffer = null;
                while (socket.Poll(10, SelectMode.SelectRead))
                {
                    //开始接收数据,如果接收到的数据为0，表示已经远程连接已经关闭
                    if (buffer == null)
                    {
                        buffer = new byte[Constants.RECEIVE_BUFFER_SIZE];
                    }

                    int count = socket.Receive(buffer, 0, buffer.Length,SocketFlags.None);
                    //更新最近接收时间
                    lastReceiveDateTime = DateTime.Now;

                    if (count == 0)
                    {
                        //读取到数据为0，表示远程连接关闭
                        //下一轮扫描时，关闭连接
                        isNeedToRemove = true;
                        ParsePacket(queue);
                        queue.Clear();
                        break;
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            queue.Add(buffer[i]);
                        }

                        if (ParsePacket(queue))
                        {
                            if (queue.Count > Constants.MAX_BAG_SIZE)
                            {
                                //注：单包超过上限，放弃该包
                                queue.Clear();
                            }
                            break;
                        }

                        if (queue.Count > Constants.MAX_BAG_SIZE)
                        {
                            //注：单包超过上限，放弃该包
                            queue.Clear();
                            break;
                        }

                    }
                }
                //重置出错次数
                errorTime = 0;
            }
            catch (SocketException socketException)
            {
                errorTime++;

                LogUtils.Error(String.Format("接收socket数据发生异常,异常代码：socketerrorcode={0}, 异常信息：{1}",socketException.SocketErrorCode,socketException));
            }
            catch(Exception ex)
            {
                errorTime++;
                LogUtils.Error("接收数据发生异常："+ex);
            }

            if (errorTime >= 2)
            {
                //第二次连续错误从队列中移除
                isNeedToRemove = true;
            }
        }

        /// <summary>
        /// 解析处理数据包，如果有合法的数据包进行解析并返回true，否则返回false
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private bool ParsePacket(List<byte> packet)
        {
            if (packet.Count == 0)
            {
                return false;
            }

            //获取终结符
            byte[] terminatorArr = Constants.DEFAULT_TERMINATOR_BYTE;
            //获取数据
            byte[] dataArr = packet.ToArray();
            int end = 0;
            int start = 0;
            //注：该算法会查找所有的可解析的消息
            for (int i = 0; i < dataArr.Length; i++)
            {
                //查找终结符
                bool found = true;
                int j = i;
                for (; (j < dataArr.Length&& j-i < terminatorArr.Length); j++)
                {
                    if (dataArr[j] != terminatorArr[j - i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found && (j-i)==terminatorArr.Length)
                {
                    //匹配到了消息结尾
                    String message = Constants.DEFAULT_ENCODING.GetString(dataArr, start, i-start);
                    MessageItem item = new MessageItem(message,this);
                    pool.AddReceiveMessage(item);
                    start = j;
                    i = j-1;
                    end = j;
                    LogUtils.Info(string.Format("{0}接收到消息： {1}",clientName,message));
                }
            }
            //从队列中移除可解析的包
            if (end > 0)
            {
                packet.RemoveRange(0, end);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


        #region send

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public bool Send(string message)
        {
            try
            {
                if (isNeedToRemove)
                {
                    return false;
                }

                byte[] buffer = Constants.DEFAULT_ENCODING.GetBytes(message + Constants.DEFAULT_TERMINATOR);
                socket.Send(buffer, 0, buffer.Length, SocketFlags.None);

                return true;
            }
            catch (Exception ex)
            {
                isNeedToRemove = true;
                LogUtils.Error(string.Format("发送消息{0}失败，异常信息:{1}",message,ex));
                return false;
            }
        }

        #endregion

        #endregion

    }
}
