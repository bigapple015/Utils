/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    客户端—>服务器端数据响应
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// 客户端--->服务器的响应
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 响应的id
        /// </summary>
        public Guid ResponseId { get; set; }

        /// <summary>
        /// 请求是否有效请求。
        /// true, if it is valid, otherwise false.
        /// </summary>
        public Boolean IsValid { get; set; }

        /// <summary>
        /// 返回结果码
        /// 0：处理成功
        /// 1：无效请求
        /// 可进行扩展
        /// </summary>
        public int ResponseCode { get; set; }

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
