/**
 * author:  cmlu
 * date:    2015年1月8日
 * desc:    请求的类型
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// 请求的类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// 测试链接，不请求任何内容
        /// </summary>
        NONE,
        /// <summary>
        /// 所有客户端的监控数据
        /// </summary>
        ALL,
        /// <summary>
        /// 指定的客户端监控数据
        /// </summary>
        LIST,
        /// <summary>
        /// 重启命令（必须指定机器列表）
        /// </summary>
        RESTART,
        /// <summary>
        /// 关闭命令（必须指定机器列表）
        /// </summary>
        SHUTDOWN,
    }
}
