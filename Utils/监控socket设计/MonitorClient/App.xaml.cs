using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Cares.Fids.Monitor.Models.Utils;
using Cares.Fids.Monitor.Models.Utils.Threads;
using MonitorClient.Models;
using MonitorClient.Utils;

namespace Cares.Fids.Monitor.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        #region Private Member

        private TimerEx _timerEx;

        /// <summary>
        /// 监控客户端是否在运行
        /// </summary>
        private volatile bool isRunning = true;

                /// <summary>
        /// 发送队列
        /// </summary>
        private BlockingCollection<MessageItem> sendingQueue = new BlockingCollection<MessageItem>();

        /// <summary>
        /// 接受队列
        /// </summary>
        private BlockingCollection<MessageItem> receiveQueue = new BlockingCollection<MessageItem>();

        /// <summary>
        /// 扫描线程，扫描socket进行接收，将接收到的消息放入receiveQueue
        /// </summary>
        private Thread scannerThread;

        /// <summary>
        /// 处理接收到的消息，即扫描接受队列，处理接收到的消息，如果必要讲处理后的结果放到sendingQueue
        /// </summary>
        private Thread receiveThread;

        /// <summary>
        /// 扫描sendingQueue，发送消息
        /// </summary>
        private Thread sendingThread;


        private SocketEx _socketEx = new SocketEx("192.168.163.204",52321);

        #region proxy
        /// <summary>
        /// 扫描代理,扫描socket接收信息
        /// </summary>
        private void ScannerProxy()
        {
            while (isRunning)
            {
                try
                {
                    if (_socketEx != null)
                    {
                        _socketEx.Receive();
                    }
                }
                catch (Exception ex)
                {
                    LogWindowUtils.LogError("接收数据发送错误："+ex);
                }

                Sleep();
            }
        }

        /// <summary>
        /// 扫描sending队列，发送消息
        /// </summary>
        private void SendingProxy()
        {
            while (isRunning)
            {
                try
                {
                    if (sendingQueue == null)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    MessageItem item = null;

                    if (sendingQueue.TryTake(out item, 100) && item != null)
                    {
                        //发送消息
                        bool isSuccess = _socketEx.Send(item.GetMessage());

                        //根据是否成功发送消息进行进一步处理


                        //根据是否成功发送消息进行进一步处理
                    }

                }
                catch (Exception ex)
                {
                    LogUtils.Error("扫描sending队列，发送消息失败："+ex);
                    Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// receiveThread：扫描receiveQueue队列，并处理接收到的消息，将要发送的消息丢到sendingQueue中
        /// </summary>
        private void ReceiveProxy()
        {
            while (isRunning)
            {
                try
                {
                    MessageItem item = null;

                    //从队列中取出一条消息
                    if (receiveQueue.TryTake(out item, 100) && item != null)
                    {
                        //注：因为使用了发送和接收队列，所以对于同一个连接，发送是不会同时进行的，接收也是不会同时进行的，所以不存在同步问题
                        item.Parse();
                    }
                }
                catch (Exception ex)
                {
                    LogWindowUtils.LogError("扫描receiveQueue队列消息，并处理消息失败：" + ex);
                    Thread.Sleep(200);
                }
            }
        }


        private void Sleep()
        {
            sleepCount++;
            if (sleepCount%5 == 0)
            {
                Thread.Sleep(200);
            }

            if (sleepCount > 100000000)
            {
                sleepCount = 1;
            }
        }
        /// <summary>
        /// 睡眠标志
        /// </summary>
        private volatile int sleepCount;
        #endregion
        #endregion

        #region public Method

        /// <summary>
        /// 获取接受队列
        /// </summary>
        /// <returns></returns>
        public BlockingCollection<MessageItem> GetReceiveQueue()
        {
            return receiveQueue;
        }

        /// <summary>
        /// 获取发送队列
        /// </summary>
        /// <returns></returns>
        public BlockingCollection<MessageItem> GetSendingQueue()
        {
            return sendingQueue;
        }

        /// <summary>
        /// 项接受队列中添加消息
        /// </summary>
        public static  bool ReceiveMessage(MessageItem messageItem)
        {
            App application = Application.Current as App;


            if (application != null && application.GetReceiveQueue() != null)
            {
                return application.GetReceiveQueue().TryAdd(messageItem);
            }

            return false;
        }

        /// <summary>
        /// 项发送队列中添加消息
        /// </summary>
        public static bool SendMessage(MessageItem messageItem)
        {
            App application = Application.Current as App;


            if (application != null && application.GetSendingQueue() != null)
            {
                return application.GetSendingQueue().TryAdd(messageItem);
            }

            return false;
        }

        #endregion

        /// <summary>
        /// 开始wpf程序时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            #region 单例
            /*
            if (!OneRunner.IsOnlyOneProcessRunning())
            {
                MessageBox.Show("监控客户端已经启动一个实例，点击确定退出本实例","提示信息",MessageBoxButton.OK,MessageBoxImage.Warning);
                System.Environment.Exit(-1);
                return;
            }
            */
            #endregion

            #region 加载配置

            ClientConfig.I = ClientConfig.Load(ConfigUtils.GetString("ClientConfigFile","ClientConfig.xml"));

            if (ClientConfig.I == null || !ClientConfig.I.IsValid())
            {
                LogWindowUtils.LogError("加载客户端配置失败");
                Task.Run(() =>
                {
                    Thread.Sleep(5000);
                    Environment.Exit(-1);
                });
                MessageBox.Show("加载客户端配置失败，请检查客户端配置.\r\n\r\n\r\n本窗口5s消失", "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            
            #endregion
            LogWindowUtils.LogInfo("使用的扫描周期是（秒为单位）："+ClientConfig.I.SingalPeriod);
            StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
            //初始化
            _socketEx.Connect();

            StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);

            _timerEx = new TimerEx(obj =>
            {
                if (isRunning)
                {
                    SendMessage(new MessageItem("Hello world"));
                }
            },5000,ClientConfig.I.SingalPeriod);

            scannerThread = new Thread(ScannerProxy);
            receiveThread = new Thread(ReceiveProxy);
            sendingThread = new Thread(SendingProxy);
            scannerThread.Start();
            sendingThread.Start();
            receiveThread.Start();
        }

        /// <summary>
        /// 注销或者关闭操作系统时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            isRunning = false;
            _timerEx.Stop();
            Thread.Sleep(100);
            _socketEx.Stop();
            LogUtils.Info("注销或者关闭操作系统");
        }

        /// <summary>
        /// 应用程序关闭前执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            isRunning = false;
            _timerEx.Stop();
            Thread.Sleep(100);
            LogUtils.Info("应用程序关闭");
            _socketEx.Stop();
        }

        /// <summary>
        /// ui线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogWindowUtils.LogError("UI线程未捕获异常："+e.Exception);
        }
    }
}
