/**
 * author:  cmlu
 * date:    2015年1月8日
 * desc:    监控页面到服务器接口
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// 监控页面--->服务器接口
    /// 请求接口
    /// 
    /// 注：这个数据包可能较长，保证至少500台机器的处理
    /// </summary>
    public class WebRequest
    {
        /// <summary>
        /// Guid 标志一次请求
        /// </summary>
        public Guid RequestId;

        /// <summary>
        /// 请求的时间
        /// </summary>
        public DateTime RequestTime;

        /// <summary>
        /// 请求的类型
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// 配合RequestType使用，指定请求操作的机器
        /// List中每一项guid标志一台机器
        /// 保证至少500台机器的处理
        /// </summary>
        public List<Guid> RequestList { get; set; } 
    }
}
