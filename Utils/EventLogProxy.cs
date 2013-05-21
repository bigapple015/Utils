using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace com.lib
{
    /// <summary>
    /// 事件日志代理类
    /// </summary>
    public class EventLogProxy
    {
        /// <summary>
        /// 注册事件源,注册日志源最好放到global.asax文件中
        /// </summary>
        private void RegisterLog()
        {
            //注册一个名称为cmlu的事件源
            if (!EventLog.SourceExists("myResource"))
            {
                EventLog.CreateEventSource("myResource", "myLog");
            }
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="source">获取或设置在写入事件日志时要注册和使用的源名称。</param>
        /// <param name="msg">错误信息</param>
        public void LogError(string source, string msg)
        {
            RegisterLog();

            EventLog log = new EventLog("myLog");
            //获取或设置在写入事件日志时要注册和使用的源名称。
            log.Source = source;
            log.WriteEntry(msg, EventLogEntryType.Error);
        }
    }
}
