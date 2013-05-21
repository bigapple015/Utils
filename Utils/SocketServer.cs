using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Com.Utility.Commons
{
    /// <summary>
    /// SocketServer类
    /// </summary>
    public class ServerSocket
    {
        #region 私有变量

        /// <summary>
        /// 标志Server是否正在运行
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// TCP服务器端监听连接
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// TCP服务器端监听端口号
        /// </summary>
        private readonly int portNum;

        /// <summary>
        /// Server运行线程
        /// </summary>
        private Thread thread;

        /// <summary>
        /// 最近一次异常信息
        /// </summary>
        private Exception _lastEx;

        #endregion

        #region 公共成员

        /// <summary>
        /// 标志Server运行状态
        /// </summary>
        public bool IsAlive
        {
            get { return thread != null && thread.IsAlive; }
        }

        /// <summary>
        /// 获取服务器端监听端口号
        /// </summary>
        public int PortNum
        {
            get { return portNum; }
        }

        /// <summary>
        /// 最近一次异常信息
        /// </summary>
        public Exception LastException
        {
            get { return _lastEx; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portNum"></param>
        public ServerSocket(int portNum)
        {
            this.portNum = portNum;
        }

        #endregion

        #region 开启服务器端

        /// <summary>
        /// 启动监听服务
        /// </summary>
        public void Start()
        {
            try
            {
                isRunning = true;
                thread = new Thread(Listen);
                thread.Start();
            }
            catch (Exception ex)
            {
                _lastEx = ex;
            }
        }

        /// <summary>
        /// 服务器端监听方法
        /// </summary>
        private void Listen()
        {
            try
            {
                //构建TCPListener,监听当前所有ip地址
                listener = new TcpListener(IPAddress.Any,portNum);
                listener.Start();
                while(isRunning)
                {
                    //检测是否有挂起的请求
                    if(!listener.Pending())
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    //接收挂起的连接
                    Socket client = listener.AcceptSocket();

                    //构建请求处理类
                    //RequestHandle requestHandle = new RequestHandle(client);
                    //调用线程池进行处理
                    //ThreadPoolHelper.ExecThreadPool(ThreadProc, requestHandle);
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                _lastEx = ex;
            }
        }

        ///// <summary>
        ///// 实际处理请求的类
        ///// </summary>
        ///// <param name="stateInfo"></param>
        //private static void ThreadProc(Object stateInfo)
        //{
        //    //设置后台线程
        //    Thread.CurrentThread.IsBackground = true;

        //    RequestHandle requestHandle = stateInfo as RequestHandle;

        //    if (requestHandle != null)
        //    {
        //        requestHandle.Handle();
        //    }
        //}

        #endregion

        #region 关闭服务器

        /// <summary>
        /// 关闭服务器端
        /// </summary>
        public void Stop()
        {
            try
            {
                isRunning = false;
                if(listener != null)
                {
                    listener.Stop();
                }
                if(this.thread != null)
                {
                    thread.Join(1000);
                }
            }
            catch (Exception ex)
            {
                _lastEx = ex;
            }
        }

        #endregion

    }

}
