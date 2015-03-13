using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Utils;
using MonitorServerConsole.Models;
using MonitorServerConsole.Utils;
using MonitorServerConsole.Utils.Sockets;

namespace MonitorServerConsole
{
    public class Program
    {
        #region 钩子函数相关

        /// <summary>
        /// 用户关闭Console时，系统发送的消息
        /// </summary>
        public const int CTRL_CLOSE_EVENT = 2;

        /// <summary>
        /// Ctrl+C,系统发送的消息
        /// </summary>
        public const int CTRL_C_EVENT = 0;

        /// <summary>
        /// Ctrl+Break,系统发送的消息
        /// </summary>
        public const int CTRL_BREAK_EVENT = 1;

        /// <summary>
        /// 用户退出(注销),系统发送的消息
        /// </summary>
        public const int CTRL_LOGOFF_EVENT = 5;

        /// <summary>
        /// 系统关闭，系统发送的消息
        /// </summary>
        public const int CTRL_SHUTDOWN_EVENT = 6;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlerRoutine"></param>
        /// <param name="add">true，表示添加；false表示删除</param>
        /// <returns>函数成功返回true,失败返回false</returns>
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);

        /// <summary>
        /// 返回false，表示程序关闭；true，不会关闭
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        public delegate bool ConsoleCtrlDelegate(int ctrlType);

        /// <summary>
        /// 保持一个引用
        /// </summary>
        public static ConsoleCtrlDelegate consoleCtrlDelegate;
        #endregion


        [STAThread]
        public static void Main(string[] args)
        {

            #region 加载控制台事件钩子
            consoleCtrlDelegate = new ConsoleCtrlDelegate(HandleConsole);
            if (SetConsoleCtrlHandler(consoleCtrlDelegate, true))
            {
                LogUtils.Info("注册控制台事件成功");
            }
            else
            {
                LogUtils.Warn("注册控制台事件失败");
            }
            #endregion

            #region 加载配置信息
            LogUtils.Info("开始加载配置文件");

            ServerConfig.I = ServerConfig.Load(ConfigUtils.GetString("ServerConfigFile", "ServerConfig.xml"));
            if (ServerConfig.I == null)
            {
                LogUtils.Error("加载配置文件失败，程序退出");
                Thread.Sleep(1000);
                return;
            }
            #endregion

            #region 启用服务器

            LogUtils.Info("开始启动服务器，");
            TcpListenerEx serverListener = new TcpListenerEx();
            if (serverListener.Start())
            {
                LogUtils.Info("启动服务器成功");
            }
            else
            {
                LogUtils.Error("启动服务器失败，程序退出");
                Thread.Sleep(1000);
                return;
            }
            #endregion

            #region 处理命令行输入

            String message = null;
            while (true)
            {
                //关闭
                Console.WriteLine("按quit退出");
                message = Console.ReadLine();
                if (message != null)
                {
                    message = message.Trim().ToLower();
                    if (message == "quit")
                    {
                        Close(serverListener);
                        break;
                    }
                    else
                    {
                        HandleCommand(message);
                    }
                }
            }

            #endregion

        }

        #region 处理控制台事件
        /// <summary>
        /// 实际处理控制台响应时间
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        private static bool HandleConsole(int ctrlType)
        {
            switch (ctrlType)
            {
                case CTRL_C_EVENT:
                case CTRL_BREAK_EVENT:
                    LogUtils.Info("请明确点击关闭按钮关闭");
                    return true;
                case CTRL_CLOSE_EVENT:
                    LogUtils.Error(String.Format("用户[{0}]于{1}点击关闭按钮关闭系统", Environment.UserName, DateTime.Now));
                    break;
                case CTRL_LOGOFF_EVENT:
                    LogUtils.Error(String.Format("用户 {0} 于 {1} 退出(注销)", Environment.UserName, DateTime.Now));
                    break;
                case CTRL_SHUTDOWN_EVENT:
                    LogUtils.Error(String.Format("用户 {0} 于 {1} 退出系统", Environment.UserName, DateTime.Now));
                    break;
            }

            //返回false，忽略处理，让系统进行默认的操作
            //返回true,阻止系统的操作
            return false;
        }
        #endregion

        #region 处理控制台输入命令
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void HandleCommand(String message)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogUtils.Error(string.Format(""));
            }

            Print();
        }

        /// <summary>
        /// 打印常用命令
        /// </summary>
        public static void Print()
        {

        }

        #endregion

        #region Close

        public static void Close(TcpListenerEx listenerEx)
        {
            if (listenerEx != null)
            {
                listenerEx.Stop();
            }
        }

        #endregion
    }
}
