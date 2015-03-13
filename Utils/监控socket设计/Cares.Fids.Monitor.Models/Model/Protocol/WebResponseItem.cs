/**
 * author:  cmlu
 * date:    2015年1月8日
 * desc:    web响应中的一项
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// web响应的一项
    /// </summary>
    public class WebResponseItem
    {
        /// <summary>
        /// 是否是有效请求。 true:是有效请求；false:不是有效请求
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 返回处理结果码
        /// 0：处理成功
        /// 1：处理失败
        /// </summary>
        public int ResponseCode { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public Guid DeviceId { get; set; }

        /// <summary>
        /// 内存状况
        /// </summary>
        public MemoryStatus MemoryInfo { get; set; }

        /// <summary>
        /// 硬盘状况
        /// </summary>
        public List<DriverStatus> DriverInfo { get; set; }

        /// <summary>
        /// cpu状况
        /// </summary>
        public CpuStatus CpuInfo { get; set; }
    }
}
