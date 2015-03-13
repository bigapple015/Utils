/**
 * author:  cmlu
 * date:    2015年1月8日
 * desc:    服务器---->监控页面接口
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
    /// 服务器--->监控页面接口
    /// </summary>
    public class WebResponse
    {
        /// <summary>
        /// guid,标志响应,通常是和请求的id一致
        /// </summary>
        public Guid ResponseId;

        /// <summary>
        /// 是否是有效请求。 true,表示是有效请求
        /// </summary>
        public bool IsValid;

        /// <summary>
        /// 响应码
        /// 0：表示成功处理请求
        /// 1：表示处理请求失败
        /// 其它值待定
        /// </summary>
        public bool ResponseCode;

        /// <summary>
        /// 每一项代表每台机器的响应
        /// 注：可能数据量较大，至少能够支持500台机器
        /// </summary>
        public List<WebResponseItem> Responses;
    }
}
