using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Utils.Sockets
{
    /// <summary>
    /// Socket帮助类
    /// </summary>
    public static class SocketHelper
    {
        /// <summary>
        /// 创建一个用于tcp通信的socket实例
        /// </summary>
        /// <returns></returns>
        public static Socket CreateTcpSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// 连接指定的服务器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool Connect(Socket socket, String ip, int port)
        {
            try
            {
                socket.Connect(IPAddress.Parse(ip), port);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Error(String.Format("尝试连接服务器{0}:{1}失败，异常信息：{2}", ip, port, ex));
                return false;
            }
        }

        /// <summary>
        /// 可多次调用的close方法
        /// </summary>
        /// <param name="socket"></param>
        public static void Close(Socket socket)
        {
            if (socket == null)
            {
                return;
            }
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {

            }
            finally
            {
                socket.Close();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <param name="withLock"></param>
        public static void Send(Socket socket, String message, Encoding encoding,bool withLock = false)
        {
            if (message == null || socket == null)
            {
                return;
            }

            byte[] buffer = encoding.GetBytes(message);

            if (!withLock)
            {
                socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
            else
            {
                lock (socket)
                {
                    socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                }
            }
        }

        /// <summary>
        /// Receive Message Fromsocket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int Receive(Socket socket,ref byte[] buffer,bool withLock = false)
        {
            if (!withLock)
            {
                return socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            }
            else
            {
                int count = 0;
                lock (socket)
                {
                    count = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                }
                return count;
            }
        }
    }
}
