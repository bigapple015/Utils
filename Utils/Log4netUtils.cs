/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    log4net日志实现
 * 
 */

using System;
using Cares.Fids.Monitor.Models.Model;

namespace Cares.Fids.Monitor.Models.Utils
{
    /// <summary>
    /// log4net日志
    /// 通过log4net配置来支持多线程
    /// 支持error\ warn\ info\ debug日志
    /// </summary>
    public static class Log4netUtils
    {
        /// <summary>
        /// 日志名
        /// </summary>
        private const string LOGGERNAME = "GlobalLog";

        #region error
        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Error(string msg)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Error(msg);
            }
            catch 
            {
                
            }
        }


        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Error(string msg, Exception exception)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Error(msg, exception);
            }
            catch 
            {
                
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).ErrorFormat(format, args);
            }
            catch
            {
                
            }
        }

        #endregion

        #region warn

        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg);
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public static void Warn(string msg,Exception exception)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Warn(msg, exception);
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 写warn日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, params object[] args)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).WarnFormat(format, args);
            }
            catch
            {
                
            }
        }

        #endregion

        #region  info
        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        public static void Info(string msg)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Info(msg);
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="msg"> The message object to log.</param>
        /// <param name="ex">The exception to log, including its stack trace.</param>
        public static void Info(string msg,Exception exception)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Info(msg, exception);
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 写info日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).InfoFormat(format, args);
            }
            catch
            {
                
            }
        }

        #endregion

        #region Debug
        
        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg);
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public static void Debug(string msg, Exception exception)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).Debug(msg, exception);
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, params object[] args)
        {
            try
            {
                log4net.LogManager.GetLogger(LOGGERNAME).DebugFormat(format, args);
            }
            catch
            {
                
            }
        }

        #endregion
    }
}
