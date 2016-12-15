using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scmis.Plc.Utils
{
    /// <summary>
    /// 同一时刻只能运行一个方法
    /// </summary>
    public class OneRunAtSameTime
    {
        /// <summary>
        /// 是否正在运行
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public void Run(Action action)
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            try
            {
                action();
            }
            finally
            {
                lock (locker)
                {
                    isRunning = false;
                }
            }
        }

        /// <summary>
        /// 同一时刻只能运行一个方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler">异常处理</param>
        public void RunSafe(Action action, Action<Exception> exceptionHandler = null)
        {
            lock (locker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            finally
            {
                lock (locker)
                {
                    isRunning = false;
                }
            }
        }
    }
}
