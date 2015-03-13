using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Utils;
using log4net.Config;
using MonitorServerConsole.Models;

namespace MonitorServerConsole.Utils.Sockets
{
    /// <summary>
    /// 作为服务器的TcpListener
    /// </summary>
    public class TcpListenerEx:IDisposable
    {
        #region Private　Member

        /// <summary>
        /// TcpListener是否在运行
        /// </summary>
        private volatile bool isRunning;

        /// <summary>
        /// TcpListener实例
        /// </summary>
        private TcpListener tcpListener;


        /// <summary>
        /// 服务器接收客户端连接所使用的线程
        /// 该线程是前台线程
        /// </summary>
        private Thread serverThread;

        /// <summary>
        /// 同步事件，确保子线程已启动
        /// </summary>
        private ManualResetEvent resetEvent;

        /// <summary>
        /// 扫描维护队列
        /// </summary>
        private Thread scannerThread;

        /// <summary>
        /// 创建的线程因子
        /// 创建的线程数 = （int）cpu*线程因子
        /// </summary>
        public const double ThreadFactor = 2;

        /// <summary>
        /// 创建客户端连接池
        /// </summary>
        private volatile ConcurrentBag<ClientPool> clientPools;

        /// <summary>
        /// 上一次扫描时间
        /// </summary>
        private DateTime lastScannerTime;
        #endregion

        #region Start

        /// <summary>
        /// 启动服务器，并返回是否成功启动
        /// </summary>
        /// <returns></returns>
        public Boolean Start()
        {
            try
            {
                #region 初始化

                lastScannerTime = DateTime.Now;
                resetEvent = new ManualResetEvent(false);
                isRunning = false;
                #endregion
                serverThread = new Thread(Listen) {Name = "ServerThread", IsBackground = false};
                serverThread.Start();
                resetEvent.WaitOne();

                if (isRunning)
                {
                    LogUtils.Info(String.Format("绑定服务器{0}成功", GetLocalEndPoint()));
                    //服务器启动成功
                    //开始启动连接池
                    int poolNumber = (int) (Environment.ProcessorCount*ThreadFactor);

                    clientPools = new ConcurrentBag<ClientPool>();
                    for (int i = 1; i <= poolNumber; i++)
                    {
                        clientPools.Add(new ClientPool(this, "连接池" + i));
                    }

                    scannerThread = new Thread(ScannerProxy);
                    scannerThread.Start();
                    LogUtils.Info(string.Format("当前机器{0}核，创建{1}个连接池", Environment.ProcessorCount, poolNumber));
                }
                //结束启动连接池

            }
            catch (Exception ex)
            {
                LogUtils.Error(String.Format("启动服务器失败：{0}",ex));
                isRunning = false;
            }
            finally
            {
                resetEvent = null;
            }
            return isRunning;
        }

        /// <summary>
        /// 监听是否有新链接接入
        /// </summary>
        private void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, ServerConfig.I.ServerPort);
                tcpListener.Start(ServerConfig.I.Backlog);
                isRunning = true;
                resetEvent.Set();

                //给主线程时间创建连接池
                Thread.Sleep(1000);
                LogUtils.Info("服务器开始接受等待新链接---------------");
                while (isRunning)
                {
                    try
                    {
                        //检查是否有挂起的请求
                        if (!tcpListener.Pending())
                        {
                            Thread.Sleep(10);
                            continue;
                        }
                        Socket socket = tcpListener.AcceptSocket();

                        #region 开始选择连接池

                        //开始选择一个连接池存放连接, 选择当前连接最少的
                        ClientPool[] arr = clientPools.ToArray();
                        ClientPool pool = arr[0];
                        for (int i = 1; i < arr.Length; i++)
                        {
                            if (arr[i].ConnectCount < pool.ConnectCount)
                            {
                                pool = arr[i];
                            }
                        }
                        //创建客户端连接代理
                        Tuple<string,int> clientName = GetRemoteEndPoint(socket);
                        ClientEx client = new ClientEx(pool,socket, clientName);

                        pool.GetClientExs().Add(client);
                        LogUtils.Info(String.Format("连接池{0} 新增来自 {1} 的连接,目前池中连接数为 {2}",pool.GetPoolName(),clientName,pool.ConnectCount));
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LogUtils.Error(String.Format("[非常严重]等待获取客户端连接失败：{0}",ex));
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                isRunning = false;
                resetEvent.Set();
                LogUtils.Error(String.Format("开启服务器监听线程失败：{0}",ex));
            }
        }

        #endregion

        #region Stop

        /// <summary>
        /// 实现IDisposable接口
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 获取服务器IP和地址
        /// </summary>
        /// <returns></returns>
        private string GetLocalEndPoint()
        {
            if (!isRunning)
            {
                return string.Empty;
            }

            try
            {
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(i=>i.AddressFamily == AddressFamily.InterNetwork).ToArray();
                if (addressList.Length == 0)
                {
                    return string.Empty;
                }
                return addressList[0]+":"+ServerConfig.I.ServerPort;
            }
            catch (Exception ex)
            {
                LogUtils.Warn("获取服务器绑定信息失败，但不影响程序执行："+ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取远程连接的ip和地址
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private Tuple<string,int> GetRemoteEndPoint(Socket socket)
        {
            try
            {
                IPEndPoint remotePoint = (IPEndPoint)socket.RemoteEndPoint;
                return new Tuple<string, int>(remotePoint.Address.ToString() , remotePoint.Port);
            }
            catch (Exception ex)
            {
                LogUtils.Warn("获取远程连接信息失败："+ex);
                return null;
            }
        }

        /// <summary>
        /// 扫描线程，扫描队列，维护连接的客户端
        /// </summary>
        private void ScannerProxy()
        {
            while (isRunning)
            {
                #region 扫描连接池观察是否有需要移除的连接
                try
                {
                    if (clientPools != null && !clientPools.IsEmpty)
                    {
                        IEnumerator<ClientPool> enumerator = clientPools.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            ClientPool pool = enumerator.Current;
                            IEnumerator<ClientEx> clientEnumerator = pool.GetClientExs().GetEnumerator();
                            while (clientEnumerator.MoveNext())
                            {
                                ClientEx client = clientEnumerator.Current;
                                if (client.IsNeedToRemove)
                                {
                                    client.ShutDown();
                                    pool.RemoveConect(client);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.Error("服务器维护出错(扫描连接池观察是否有需要移除的连接)：" + ex);
                }
                #endregion

                #region  周期性检查连接是否真正断开

                try
                {
                    if (DateTime.Now.Subtract(lastScannerTime).TotalSeconds > ServerConfig.I.ScannerPeriod)
                    {
                        lastScannerTime = DateTime.Now;
                        if (clientPools != null && !clientPools.IsEmpty)
                        {
                            IEnumerator<ClientPool> enumerator = clientPools.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                ClientPool pool = enumerator.Current;
                                IEnumerator<ClientEx> clientEnumerator = pool.GetClientExs().GetEnumerator();
                                while (clientEnumerator.MoveNext())
                                {
                                    ClientEx client = clientEnumerator.Current;
                                    if (!client.IsNeedToRemove &&
                                        (DateTime.Now.Subtract(client.GetLastVisitTime())).TotalSeconds >
                                        ServerConfig.I.ScannerPeriod)
                                    {
                                        //需要发送一次消息检验是否连接断开
                                        String message = "";
                                        MessageItem item = new MessageItem(message,client);
                                        client.GetClientPool().AddSendingMessage(item);
                                    }
                                }
                            }
                        }
                    } 
                }
                catch (Exception ex)
                {
                    LogUtils.Error("服务器维护出错(周期性检查连接是否真正断开)：" + ex);
                }

                #endregion

                //休息
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                }
            }
        }

        #endregion

        #region public Method

        /// <summary>
        /// 服务器是否运行
        /// 即，服务器是否可以接受连接，处理请求
        /// </summary>
        public bool IsRunning
        {
            get { return isRunning; }
        }

        /// <summary>
        /// 获取所有链接的数目
        /// </summary>
        /// <returns></returns>
        public int GetConnectNumber()
        {
            if (clientPools == null)
            {
                return 0;
            }

            return clientPools.Sum(p => p.ConnectCount);
        }

        #endregion

    }
}
