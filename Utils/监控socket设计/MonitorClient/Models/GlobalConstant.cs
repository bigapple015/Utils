/**
 * author     : cmlu
 * date       : 2015年1月6日 
 * description：全局常量类
 */

using System;

namespace Cares.Fids.Monitor.Models.Model
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public static class GlobalConstant
    {
        /// <summary>
        /// 以此来确认tcp的请求
        /// </summary>
        public const String RequestTerminator = "\r\n\r\n##";

        /// <summary>
        /// 日志名
        /// </summary>
        public const String LoggerName = "GlobalLog";
    }
}
