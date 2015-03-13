/**
 * author:  cmlu
 * data:    2015年1月7日
 * desc:    放置常量
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model
{
    /// <summary>
    /// 常量类
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// 默认的编码规则
        /// </summary>
        public static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;

        /// <summary>
        /// 日志名
        /// </summary>
        public const String LOGGER_NAME = "GlobalLog";

        /// <summary>
        /// 默认终止符
        /// </summary>
        public const String DEFAULT_TERMINATOR = "##<eof>##";

        /// <summary>
        /// 默认终止符
        /// </summary>
        public static readonly byte[] DEFAULT_TERMINATOR_BYTE = DEFAULT_ENCODING.GetBytes(DEFAULT_TERMINATOR);

        /// <summary>
        /// 接收的buffer大小 = 1024*8b  8k
        /// 设置为8k,因为socket默认的接收缓冲是8k
        /// </summary>
        public const int RECEIVE_BUFFER_SIZE = 1024*8;

        /// <summary>
        /// 单包不超过2M
        /// </summary>
        public const int MAX_BAG_SIZE = 1024*1024*2;

        /// <summary>
        /// 发送超时时间，以毫秒为单位
        /// </summary>
        public const int SENDTIMEOUT = 5000;

        /// <summary>
        /// 接收超时时间，以毫秒为单位
        /// </summary>
        public const int RECEIVETIMEOUT = 5000;

    }
}
