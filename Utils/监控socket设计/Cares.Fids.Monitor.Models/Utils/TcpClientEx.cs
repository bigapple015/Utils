/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    tcp的客户端
 * 使用tcp的客户端
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Model;

namespace Cares.Fids.Monitor.Models.Utils
{
    /// <summary>
    /// tcpclient扩展
    /// </summary>
    public class TcpClientEx
    {

        #region Member

        /// <summary>
        /// a tcp client
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// the ip you want to connect (the ip of server)
        /// </summary>
        private string ip;

        /// <summary>
        /// the port you want to connect
        /// </summary>
        private int port;

        /// <summary>
        /// 传输的编码
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// the tcp client is connected or not
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// 最近发生的异常
        /// </summary>
        private Exception lastException;

        /// <summary>
        /// 终止符
        /// </summary>
        private byte[] terminator;


        #region buffer

        /// <summary>
        /// the buffer to receive
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// 接收到数据的offset
        /// </summary>
        private int offset;

        #endregion

        #endregion

        #region constructor

        /// <summary>
        /// set the ip & port using constructor
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="encoding">默认使用UTF-8</param>
        public TcpClientEx(String ip, int port, string encoding = "UTF-8")
        {
            this.ip = ip;
            this.port = port;
            this.encoding = Encoding.GetEncoding(encoding);
            terminator = this.encoding.GetBytes(Constants.DEFAULT_TERMINATOR);
            buffer = new byte[Constants.RECEIVE_BUFFER_SIZE];
            offset = 0;
        }

        #endregion

        #region connect

        /// <summary>
        /// 连接到远程端
        /// </summary>
        public void Connect()
        {
            //如果已经连接，先关闭连接
            if (isConnected)
            {
                Close();
            }

            //重新构建一个连接
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpClient = new TcpClient();
            //使用默认的sendbuffersize , receivebuffersize = 8192
            tcpClient.SendTimeout = LocalConfig.I.SendTimeout;
            tcpClient.ReceiveTimeout = LocalConfig.I.ReceiveTimeout;
            tcpClient.Connect(endPoint);
            isConnected = true;
        }

        /// <summary>
        /// connect to remote without throw any exception
        /// </summary>
        /// <returns>true, if successfully connected without exception; false, otherwise</returns>
        public bool ConnectSafe()
        {
            try
            {
                Connect();
                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                isConnected = false;
                return false;
            }
        }

        #endregion

        #region close

        /// <summary>
        /// close the connection
        /// </summary>
        public void Close()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
            isConnected = false;
        }

        #endregion

        #region send

        /// <summary>
        /// 发送消息，如果发送失败抛出异常抛出异常
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg)
        {
            if (msg == null)
            {
                msg = string.Empty;
            }

            byte[] bytes = encoding.GetBytes(msg);
            tcpClient.GetStream().Write(bytes,0,bytes.Length);
        }

        /// <summary>
        /// 发送消息，如果发送失败也不抛出异常
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendSafe(string msg)
        {
            try
            {
                Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        #endregion

        #region receive

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns>true, 如果解析了包；false,如果解析失败</returns>
        public Tuple<bool,string> receive()
        {
            try
            {
                offset = 0;
                NetworkStream stream = tcpClient.GetStream();
                Tuple<bool, int> tuple = null;
                while (true)
                {
                    int count = stream.Read(buffer, offset, buffer.Length - offset);

                    offset += count;
                    tuple = search();
                    if (tuple.Item1)
                    {
                        //找到了完整包
                        break;
                    }
                    else if (offset >= buffer.Length)
                    {
                        //buffer满了，为了防止内存占用过多和攻击，放弃解析本包
                        break;
                    }
                }

                if (tuple.Item1)
                {
                    return new Tuple<bool, string>(true, encoding.GetString(buffer, 0, tuple.Item2));
                }
                else
                {
                    return new Tuple<bool, string>(false, string.Empty);
                }
            }
            catch (Exception ex)
            {
                this.lastException = ex;
                return new Tuple<bool, string>(false, string.Empty);
            }
        }

        /// <summary>
        /// 在接收到的buffer中查找终止符
        /// </summary>
        /// <returns>true,如果找到，其结束位置为第二个参数；false如果未找到，第二个参数没有意义</returns>
        private Tuple<bool, int> search()
        {
            for (int i = 0; i < offset; i++)
            {
                if (buffer[i] == terminator[0])
                {
                    bool isSame = true;

                    for (int j = 0; j < terminator.Length; j++)
                    {
                        if ((i + j) >= offset)
                        {
                            //超出边界，不可能相同，返回false
                            return new Tuple<bool, int>(false, 0);
                        }
                        //对应位置不相等,i不是要查找的位置，退出循环
                        if (buffer[i + j] != terminator[j])
                        {
                            isSame = false;
                            break;
                        }
                    }
                    //找到索引位置
                    if (isSame)
                    {
                        return new Tuple<bool, int>(true,i);
                    }
                }
            }

            return new Tuple<bool, int>(false,0);
        }

        #endregion

        #region gettcpclient

        /// <summary>
        /// 获取tcpClient
        /// </summary>
        /// <returns></returns>
        public TcpClient GetTcpClient()
        {
            return tcpClient;
        }

        #endregion

        #region isConnected

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return isConnected;
        }

        #endregion

    }
}
