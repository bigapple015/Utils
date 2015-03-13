using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Cares.Fids.Monitor.Client;
using Cares.Fids.Monitor.Models.Utils;

namespace MonitorClient.Utils
{
    public static class LogWindowUtils
    {
        private static ConcurrentQueue<String> infoBuffer = new ConcurrentQueue<string>();
        private static ConcurrentQueue<String> errorBuffer = new ConcurrentQueue<string>();
        private const int MaxLineCount = 128;

        /// <summary>
        /// 窗口
        /// </summary>
        /// <param name="message"></param>
        private static void LogWindow(string message)
        {
            try
            {
                infoBuffer.Enqueue(DateTime.Now.ToString(@"yyyy'-'MM'-'dd' 'HH':'mm':'ss") + ":  " +message);
                if (infoBuffer.Count > MaxLineCount)
                {
                    string s = null;
                    for (int i = 0; i < MaxLineCount/10; i++)
                    {
                        infoBuffer.TryDequeue(out s);
                    }
                }
                StringBuilder sb = new StringBuilder();
                IEnumerator<String> enumerator = infoBuffer.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    sb.AppendLine(enumerator.Current);
                }

                Application.Current.Dispatcher.BeginInvoke(new Action<String>(ShowLog), DispatcherPriority.Normal,sb.ToString());
                
            }
            catch (Exception ex)
            {
                LogUtils.Error("记录窗口日志失败，异常信息为："+ex);
            }
        }


        public static void LogInfo(string message)
        {
            LogWindowUtils.LogWindow(message);
            LogUtils.Info(message);
        }

        public  static void LogError(string message)
        {
            LogWindowUtils.LogWindow(message);
            LogUtils.Error(message);
        }

        private static void ShowLog(string message)
        {
            Window window = Application.Current.MainWindow;
            if (window != null && window is MainWindow)
            {
                MainWindow mainWindow = (MainWindow) window;
                mainWindow.ShowLog(message);
            }
        }
    }
}
