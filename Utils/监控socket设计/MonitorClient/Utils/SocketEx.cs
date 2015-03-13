using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Client;
using Cares.Fids.Monitor.Models.Model;
using Cares.Fids.Monitor.Models.Utils;
using Cares.Fids.Monitor.Models.Utils.Sockets;
using MonitorClient.Models;

namespace MonitorClient.Utils
{
    public class SocketEx : IDisposable
    {
        #region private Member
        /// <summary>
        /// 底层socket
        /// </summary>
        private Socket socket;

        /// <summary>
        /// socket状态
        /// </summary>
        private volatile SocketStatus socketStatus;

        /// <summary>
        /// 服务器ip
        /// </summary>
        private volatile String serverIp;

        /// <summary>
        /// 服务器端口
        /// </summary>
        private volatile int serverPort;

        /// <summary>
        /// 连续错误次数
        /// </summary>
        private volatile int errorCount;

        /// <summary>
        /// 连续错误次数
        /// </summary>
        private const int ErrorCountLimit = 3;

        /// <summary>
        /// 消息队列解析
        /// </summary>
        private List<byte> queue; 

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        public SocketEx(string serverIp, int serverPort)
        {
            this.serverIp = serverIp;
            this.serverPort = serverPort;
            socketStatus = SocketStatus.UnInited;
            errorCount = 0;
            queue = new List<byte>();
        }


        public SocketEx()
        {
            socketStatus = SocketStatus.UnInited;
            errorCount = 0;
            queue = new List<byte>();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        public bool Connect()
        {
            try
            {
                lock (this)
                {
                    if (socket != null)
                    {
                        Dispose();
                    }

                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
                    socket = SocketHelper.CreateTcpSocket();
                    socket.Connect(endPoint);
                    socketStatus = SocketStatus.Inited;

                    //潜在bug,在重连时，有极低的概率会清空已经接受的数据
                    //程序正常运行不会出错
                    queue.Clear();
                }
                errorCount = 0;
                String message = String.Format("初始化服务器 {0}:{1}的连接成功", serverIp, serverPort);
                LogWindowUtils.LogInfo(message);
                return true;
            }
            catch (Exception ex)
            {
                socketStatus = SocketStatus.Closable;
                String message = String.Format("ERROR: 初始化服务器 {0}:{1}的连接失败:{2}",serverIp,serverPort,ex);
                LogWindowUtils.LogError(message);
                errorCount++;
                return false;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns>是否发送成功</returns>
        public bool Send(String message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return true;
            }
            try
            {
                //将消息+消息结尾标志发送出去
                byte[] buffer = Constants.DEFAULT_ENCODING.GetBytes(message + Constants.DEFAULT_TERMINATOR);
                socket.Send(buffer,0,buffer.Length,SocketFlags.None);
                errorCount = 0;
                LogWindowUtils.LogInfo("发送到服务器消息："+message);
                socketStatus = SocketStatus.Inited;
                return true;
            }
            catch (Exception ex)
            {
                Correct();
                String error = String.Format("发送消息失败:{0}", ex);
                LogWindowUtils.LogError(error);
                return false;
            }
        }

        /// <summary>
        /// 更正
        /// </summary>
        public void Correct()
        {
            if (errorCount++ >= ErrorCountLimit)
            {
                socketStatus = SocketStatus.Closable;
                //Connect已经维护了errorCount，重连服务器
                Connect();
                Thread.Sleep(200);
            }

            if (errorCount > 100000000)
            {
                errorCount = ErrorCountLimit+10;//防止溢出
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public void Receive()
        {
            try
            {
                if (socketStatus != SocketStatus.Inited)
                {
                    Thread.Sleep(200);
                    return;
                }

                byte[] buffer = null;

                while (socket.Poll(100, SelectMode.SelectRead))
                {
                    if (buffer == null)
                    {
                        buffer = new byte[Constants.RECEIVE_BUFFER_SIZE];
                        int count = socket.Receive(buffer, 0, buffer.Length,SocketFlags.None);

                        if (count == 0)
                        {
                            //远程服务器已经关闭
                            socketStatus = SocketStatus.Closable;
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
                }
            }
            catch (Exception ex)
            {
                socketStatus = SocketStatus.Closable;
                LogUtils.Error("接收消息失败："+ex);
                Thread.Sleep(200);
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
                for (; (j < dataArr.Length && j - i < terminatorArr.Length); j++)
                {
                    if (dataArr[j] != terminatorArr[j - i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found && (j - i) == terminatorArr.Length)
                {
                    //匹配到了消息结尾
                    String message = Constants.DEFAULT_ENCODING.GetString(dataArr, start, i - start);

                    MessageItem item = new MessageItem(message);
                    
                    //添加到接收消息队列

                    App.ReceiveMessage(item);

                    start = j;
                    i = j - 1;
                    end = j;
                    LogUtils.Info(string.Format("接收到消息： {0}", message));
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

        #region public method

        /// <summary>
        /// 服务器ip
        /// </summary>
        public string ServerIp { get { return serverIp;} set { serverIp = value; }}

        /// <summary>
        /// 获取设置服务器端口
        /// </summary>
        public int ServerPort { get { return serverPort; } set { serverPort = value; } }

        #endregion

        /// <summary>
        /// IDisposable接口
        /// </summary>
        public void Dispose()
        {
            if (socketStatus == SocketStatus.Closable)
            {
                Stop();
            }
        }


        public void Stop()
        {
            SocketHelper.Close(socket);
            socketStatus = SocketStatus.Closed;
            socket = null;
        }
    }

    public enum SocketStatus
    {
        UnInited,
        Inited,
        Closable,
        Closed
    }
}
