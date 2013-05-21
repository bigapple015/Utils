using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

namespace Com.Utility.Commons
{
    public class NetHelper
    {
        /// <summary>
        /// 验证ip地址是否正确
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIp(string ip)
        {
            if(string.IsNullOrEmpty(ip))
            {
                return false;
            }

            ////清除要验证字符串的空格
            //ip = ip.Trim();
            //模式字符串
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            return Regex.IsMatch(ip, pattern);
        }

        /// <summary>
        /// 验证是否是正确的端口号
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsValidPort(string iport,out int oport)
        {
            oport = -1;

            if(string.IsNullOrEmpty(iport))
            {
                return false;
            }

            //最小有效端口
            const int MINPORT = 501;
            //最大有效端口
            const int MAXPORT = 65535;

            if(int.TryParse(iport,out oport) && oport>= MINPORT && oport <= MAXPORT)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将字符串转换为IPAddress对象
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress StringToIpAddress(string ip)
        {
            return IPAddress.Parse(ip);
        }

        /// <summary>
        /// 获取本机的计算机名
        /// </summary>
        public static string LoaclHostName
        {
            get { return Dns.GetHostName(); }
        }

        #region 获取本机的局域网IP

        /// <summary>
        /// 获取本机的局域网ip
        /// </summary>
        public static string LANIP
        {
            get
            {
                //获取本机的ip列表，Ip列表中第一项是局域网ip，第二项是广域网ip
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                
                if(addressList == null || addressList.Length <1)
                {
                    return "";
                }

                //返回本机局域网ip
                return addressList[0].ToString();
            }
        }
        #endregion


        #region 获取本机在Internet网络的广域网IP

        /// <summary>
        /// 获取本机在Internet网络的广域网IP
        /// </summary>
        public static string WANIP
        {
            get
            {
                //获取本机的ip列表，Ip列表中第一项是局域网ip，第二项是广域网ip
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                if (addressList == null || addressList.Length < 2)
                {
                    return "";
                }

                //返回本机广域网ip
                return addressList[1].ToString();
            }
        }

        #endregion

        #region 获取远程客户机的ip

        /// <summary>
        /// 获取远程客户机的ip
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        public static string GetClientIP(Socket clientSocket)
        {
            IPEndPoint client = (IPEndPoint) clientSocket.RemoteEndPoint;
            return client.Address.ToString()+":"+client.Port;
        }

        #endregion

        #region 创建一个IPEndPoint对象


        /// <summary>
        /// 创建一个IPEndPoint对象
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint CreateIPEndPoint(string ip,int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            return new IPEndPoint(ipAddress,port);
        }

        #endregion

        #region 创建一个TcpListener对象

        /// <summary>
        /// 创建一个TcpListener对象
        /// </summary>
        /// <returns></returns>
        public static TcpListener CreateTcpListener(int port)
        {
            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress,port);

            return new TcpListener(localEndPoint);
        }
        #endregion

        #region 创建一个基于TCP协议的Socket对象

        /// <summary>
        /// 创建一个基于TCP协议的Socket对象
        /// </summary>
        /// <returns></returns>
        public static Socket CreateTcpSocket()
        {
            return new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        }

        #endregion

        #region 创建一个基于UDP协议的Socket对象

        /// <summary>
        /// 创建一个基于UDP协议的Socket对象
        /// </summary>
        /// <returns></returns>
        public static Socket CreateUdpSocket()
        {
            return new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
        }


        #endregion

        #region 获取TcpListener对象的本地终结点

        /// <summary>
        ///  获取TcpListener对象的本地终结点
        /// </summary>
        /// <param name="tcpListener"></param>
        /// <returns></returns>
        public static IPEndPoint GetLocalPoint(TcpListener tcpListener)
        {
            return (IPEndPoint) tcpListener.LocalEndpoint;
        }

        /// <summary>
        /// 获取TcpListener对象本地终结点的ip地址
        /// </summary>
        /// <param name="tcpListener"></param>
        /// <returns></returns>
        public static string GetLocalPoint_IP(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint) tcpListener.LocalEndpoint;
            return localEndPoint.Address.ToString();
        }

        /// <summary>
        /// 获取TcpListener对象的本地终结点的端口号
        /// </summary>
        /// <param name="tcpListener"></param>
        /// <returns></returns>
        public static int GetLocalPoint_Point(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint) tcpListener.LocalEndpoint;
            return localEndPoint.Port;
        }


        #endregion

        #region 获取Socket对象的本地终结点

        /// <summary>
        /// 获取Socket对象的本地终结点
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static IPEndPoint GetLocalPoint(Socket socket)
        {
            return (IPEndPoint) socket.LocalEndPoint;
        }


        /// <summary>
        /// 获取Socket对象的本地终结点的IP地址
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string GetLocalPoint_IP(Socket socket)
        {
            IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
            return localEndPoint.Address.ToString();
        }


        /// <summary>
        /// 获取Socket对象的本地终结点的端口号
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static int GetLocalPoint_Point(Socket socket)
        {
            IPEndPoint localEndPoint = (IPEndPoint) socket.LocalEndPoint;
            return localEndPoint.Port;
        }

        #endregion

        #region 绑定终结点

        /// <summary>
        /// 绑定终结点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="endPoint"></param>
        public static void BindEndPoint(Socket socket,IPEndPoint endPoint)
        {
            if(!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }

        /// <summary>
        /// 绑定终结点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="port"></param>
        public static void BindEndPoint(Socket socket,int port)
        {
            //创建终结点
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any,port);

            //绑定终结点
            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }

        /// <summary>
        /// 指定Socket对象执行监听，默认允许的最大连接数为100
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="port"></param>
        public static void StartListen(Socket socket,int port)
        {
            //创建本地终结点
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Any,port);

            if(!socket.IsBound)
            {
                socket.Bind(localPoint);
            }

            //开始监听
            socket.Listen(100);
        }

        /// <summary>
        /// 指定Socket对象执行鉴定
        /// </summary>
        /// <param name="socket">执行监听的Socket对象</param>
        /// <param name="port">监听的端口号</param>
        /// <param name="maxConnection">允许的最大挂起连接数</param>
        public static void StartListen(Socket socket,int port,int maxConnection)
        {
            //创建本地终结点
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Any,port);

            //绑定本地终结点
            if(!socket.IsBound)
            {
                socket.Bind(localPoint);
            }

            //开始监听
            socket.Listen(maxConnection);
        }

        /// <summary>
        /// 创建Socket对相关执行监听
        /// </summary>
        /// <param name="socket">执行监听的socket对象</param>
        /// <param name="ip">监听的ip地址</param>
        /// <param name="port">监听的端口号</param>
        /// <param name="maxConnection">允许最大挂起连接数</param>
        public static void StartListen(Socket socket,string ip,int port,int maxConnection)
        {
            //绑定到本地终结点
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Parse(ip),port);

            //绑定到终结点
            if(!socket.IsBound)
            {
                socket.Bind(localPoint);
            }

            //开始监听
            socket.Listen(maxConnection);
        }

        #endregion

        #region 连接到基于TCP协议的服务器

        /// <summary>
        /// 连接到基于TCP协议的服务器，连接成功返回true，否则返回false
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool Connect(Socket socket,string ip,int port)
        {
            try
            {
                //连接到服务器
                socket.Connect(IPAddress.Parse(ip),port);

                //检测连接状态，并无限等待，直至响应
                return socket.Poll(-1, SelectMode.SelectWrite);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region 已同步的方式发送消息

        /// <summary>
        /// 以同步方式向指定的socket对象发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        public static void SendMsg(Socket socket,byte[] msg)
        {
            //发送消息
            socket.Send(msg, msg.Length, SocketFlags.None);
        }

        /// <summary>
        /// 已同步的方式发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        /// <param name="encoding"></param>
        public static void SendMsg(Socket socket,string msg,Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(msg);

            socket.Send(buffer, buffer.Length, SocketFlags.None);
        }

        #endregion

        #region 以同步的方式接收消息

        /// <summary>
        /// 以同步的形式接受信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">接收新的的缓冲区</param>
        /// <returns>实际接收到的字节数</returns>
        public static int ReceiveMsg(Socket socket,byte[] buffer)
        {
            return socket.Receive(buffer,0,buffer.Length,SocketFlags.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string ReceiveMsg(Socket socket,Encoding encoding)
        {
            if(socket == null || encoding ==null)
            {
                return null;
            }
            try
            {
                //6s内没有数据可读
                if(!socket.Poll(6000000,SelectMode.SelectRead))
                {
                    return null;
                }
                
                Byte[] recvBytes = new byte[socket.Available];
                int i = socket.Receive(recvBytes, recvBytes.Length, 0);
                if(i<=0)
                {
                    return null;
                }
                else
                {
                    return encoding.GetString(recvBytes, 0, i);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 关闭基于Tcp协议的socket对象

        /// <summary>
        /// 关闭基于Tcp协议的socket对象
        /// </summary>
        /// <param name="socket"></param>
        public static void Close(Socket socket)
        {
            if(socket == null)
            {
                return;
            }
            try
            {
                //进制Socket发送和接收数据
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                socket.Close();
            }
        }

        #endregion
    }
}
