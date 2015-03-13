/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    配置信息
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
    /// 配置信息
    /// </summary>
    public class LocalConfig
    {
        #region private member;

        /// <summary>
        /// 接收数据超时时间
        /// </summary>
        private int receiveTimeout = 1500;

        /// <summary>
        /// 发送数据超时时间
        /// </summary>
        private int sendTimeout = 2000;

        #endregion


        #region public member

        /// <summary>
        /// 接收数据超时时间
        /// </summary>
        public int ReceiveTimeout { get { return receiveTimeout; } set { receiveTimeout = value; } }

        /// <summary>
        /// 发送数据超时时间
        /// </summary>
        public int SendTimeout { get { return sendTimeout; } set { sendTimeout = value; } }

        #endregion


        public static LocalConfig I = new LocalConfig();
    }
}
