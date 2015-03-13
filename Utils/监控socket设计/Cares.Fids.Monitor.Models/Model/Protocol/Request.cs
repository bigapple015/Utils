/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    请求  服务器-->客户端协议
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// 请求  服务器-->客户端协议
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 表示请求的id，使用GUID来标识一次请求
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 请求内容
        /// <= 0:不请求任何内容，可用于心跳测试；1:请求cpu；2：请求内存；4：请求硬盘数据；如果请求多组数据，可使用组合，如3表示请求cpu和内存情况，8重启，16关闭
        /// </summary>
        public int RequestContent { get; set; }
    }
}
