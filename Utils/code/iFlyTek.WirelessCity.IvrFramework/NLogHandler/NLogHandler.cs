using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace NLogHandler
{
    /// <summary>
    /// Nlog日志类
    /// </summary>
    public static class NLogHelper
    {

        /// <summary>
        /// 私有的logger类
        /// </summary>
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static Logger _logger = LogManager.GetLogger("fileandconsole");

        /// <summary>
        /// 获取指定名字的日志
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void SetLoggerName(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// 记录Trace日志
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            _logger.Trace(message);
        }

        /// <summary>
        /// 记录Debug日志
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                return;
            }
            _logger.Debug(message);
        }

        /// <summary>
        /// 记录Warn日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                return;
            }
            _logger.Warn(message);
        }

        public static void Info(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                return;
            }
            try
            {
                _logger.Info(message);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 记录Error日志
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                return;
            }
            _logger.Error(message);
        }

        /// <summary>
        /// 记录Fetal日志
        /// </summary>
        /// <param name="message"></param>
        public static void Fetal(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                return;
            }
            _logger.Fatal(message);
        }
    }
}
