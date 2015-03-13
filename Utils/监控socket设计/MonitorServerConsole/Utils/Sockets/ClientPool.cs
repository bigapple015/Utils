using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Utils;
using MonitorServerConsole.Models;

namespace MonitorServerConsole.Utils.Sockets
{
    /// <summary>
    /// 客户端池
    /// 
    /// 使用连接池维护
    /// 
    /// 
    /// 每个客户端池有1个池：
    /// 该池维护客户端的连接
    /// ////////////////////////
    /// 每个客户端池需要两个队列：
    /// 队列1：保存要发送的消息
    /// 队列2：保存接收到的消息
    /// ///////////////////////////
    /// 每个客户端池同时开启三个线程：（该线程默认是前台线程）
    /// 线程1scanningThread：扫描池中所有的客户端，并接受消息，将接收到的消息保存到receiveQueue中去
    /// 线程2receiveThread：扫描receiveQueue队列，并处理接收到的消息，将要发送的消息丢到sendingQueue中
    /// 线程3sendingThread：扫描sendingQueue队列，并发送消息

    /// </summary>
    public class ClientPool
    {
        #region Private Member

        /// <summary>
        /// 客户端包
        /// </summary>
        private ConcurrentBag<ClientEx> clientBag;

        /// <summary>
        /// 底层使用队列 FIFO  的消息发送队列
        /// </summary>
        private BlockingCollection<MessageItem> sendingQueue;

        /// <summary>
        /// 接收到的信息队列
        /// </summary>
        private BlockingCollection<MessageItem> receiveQueue;

        /// <summary>
        /// 线程1scanningThread：扫描池中所有的客户端，并接受消息，将接收到的消息保存到receiveQueue中去
        /// </summary>
        private Thread scanningThread;

        /// <summary>
        /// 线程2receiveThread：扫描receiveQueue队列，并处理接收到的消息，将要发送的消息丢到sendingQueue中
        /// </summary>
        private Thread receiveThread;

        /// <summary>
        /// 线程3sendingThread：扫描sendingQueue队列，并发送消息
        /// </summary>
        private Thread sendingThread;

        /// <summary>
        /// 客户端池所在的server
        /// </summary>
        private readonly TcpListenerEx serverListener;

        /// <summary>
        /// 该连接池的名字
        /// </summary>
        private readonly String poolName;

        /// <summary>
        /// 休眠计数器
        /// </summary>
        private volatile int sleepCount;
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientPool(TcpListenerEx tcpListenerEx,String poolName)
        {
            this.poolName = poolName;
            //客户端池所在的客户端
            serverListener = tcpListenerEx;

            //初始化连接池
            clientBag = new ConcurrentBag<ClientEx>();
            //初始化发送、接收队列
            sendingQueue = new BlockingCollection<MessageItem>();
            receiveQueue = new BlockingCollection<MessageItem>();
            //客户端池所在的客户端
            serverListener = tcpListenerEx;

            //初始化扫描线程、发送线程、接收线程
            scanningThread = new Thread(ScanningProxy);
            receiveThread = new Thread(ReceiveProxy);
            sendingThread = new Thread(SendingProxy);

            scanningThread.Start();
            receiveThread.Start();
            sendingThread.Start();
            LogUtils.Info(String.Format("连接池 [{0}]初始化成功",poolName));
        }

        #endregion

        #region Public Member
        /// <summary>
        /// 该连接池中保存的连接数，近似准确
        /// </summary>
        public int ConnectCount
        {
            get { return clientBag.Count; }
        }

        #endregion

        #region Public Method -- 线程代理方法

        /// <summary>
        /// sendingThread：扫描sendingQueue队列，并发送消息
        /// </summary>
        public void SendingProxy()
        {
            while (serverListener.IsRunning)
            {
                try
                {
                    MessageItem item = null;
                    //从队列中取出一条消息,并且消息不为空
                    if (sendingQueue.TryTake(out item, 100) && item != null)
                    {
                        //注：因为使用了发送和接收队列，所以对于同一个连接，发送是不会同时进行的，接收也是不会同时进行的，所以不存在同步问题
                        bool isSuccess = item.Client.Send(item.Message);

                        //根据是否发送成功进行处理
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.Error("sendingThread扫描sendingQueue发送队列,发送消息错误：" + ex);
                    Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// 接受代理，线程2receiveThread：扫描receiveQueue队列，并处理接收到的消息，将要发送的消息丢到sendingQueue中
        /// </summary>
        public void ReceiveProxy()
        {
            while (serverListener.IsRunning)
            {
                try
                {
                    MessageItem item = null;

                    //从队列中取出一条消息
                    while (receiveQueue.TryTake(out item, 100) && item != null)
                    {
                        //注：因为使用了发送和接收队列，所以对于同一个连接，发送是不会同时进行的，接收也是不会同时进行的，所以不存在同步问题
                        item.Parse();
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.Error("扫描receiveQueue队列消息，并处理消息失败："+ex);
                    Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// 扫描代理，扫描池中所有的客户端，并接受消息，将接收到的消息保存到receiveQueue中去
        ///从队列中接收消息
        /// </summary>
        public void ScanningProxy()
        {
            while (serverListener.IsRunning)
            {
                try
                {
                    if (clientBag.IsEmpty)
                    {
                        //注：还没有连接连到客户端
                        Thread.Sleep(100);
                        continue;
                    }

                    //扫描连接池，并试图接收消息，并将消息放到消息队列
                    //该枚举表示clientBag调用时刻的快照，可以安全地与包的读取和写入一起使用。 
                    IEnumerator<ClientEx> enumerator= clientBag.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.Select();
                    }

                    //休眠策略
                    sleepCount ++;
                    if (sleepCount%4 == 0)
                    {
                        Thread.Sleep(200);
                    }
                    if (sleepCount > 100000000)
                    {
                        sleepCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.Error("扫描所有客户端连接，并试图接收消息时失败："+ex);
                    Thread.Sleep(100);
                }
            }

            LogUtils.Info("服务器停止，同时停止扫描客户端发送消息。");
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取所有客户端连接
        /// </summary>
        /// <returns></returns>
        public ConcurrentBag<ClientEx> GetClientExs()
        {
            return clientBag;
        }

        /// <summary>
        /// 获取链接名
        /// </summary>
        /// <returns></returns>
        public string GetPoolName()
        {
            return poolName;
        }

        /// <summary>
        /// 获取所在的服务器
        /// </summary>
        /// <returns></returns>
        public TcpListenerEx GetServer()
        {
            return serverListener;
        }

        /// <summary>
        /// 获取接收消息队列
        /// </summary>
        /// <returns></returns>
        public BlockingCollection<MessageItem> GetReceiveQueue()
        {
            return receiveQueue;
        }

        /// <summary>
        /// 获取发送消息队列
        /// </summary>
        /// <returns></returns>
        public BlockingCollection<MessageItem> GetSendingQueue()
        {
            return sendingQueue;
        }


        public void RemoveConect(ClientEx client)
        {
            if (clientBag.TryTake(out client))
            {
                LogUtils.Info(String.Format("{0} 从 {1} 中移除",client.GetClientName(),this.GetPoolName()));
            }
        }


        /// <summary>
        /// 向接收队列添加接收到的消息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddReceiveMessage(MessageItem item)
        {
            try
            {
                receiveQueue.Add(item);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Error(string.Format("向 {0} 中添加接收到的消息失败：{1}",poolName,ex));
                return false;
            }
        }

        /// <summary>
        /// 向发送队列添加要发送的消息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddSendingMessage(MessageItem item)
        {
            try
            {
                sendingQueue.Add(item);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Error(string.Format("向 {0} 中添加要发送的消息失败：{1}",poolName,ex));
                return false;
            }
        }
        #endregion
    }
}
