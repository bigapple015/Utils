/**
 * author:  cmlu
 * date:    2015年1月8日
 * desc:    硬盘状况
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// 硬盘状况
    /// </summary>
    public class DriverStatus
    {
        /// <summary>
        /// 磁盘名  如c\d\e等
        /// </summary>
        public String DriverName { get; set; }

        /// <summary>
        /// 使用的存储空间 （以字节为单位）
        /// </summary>
        public long UsedStorage { get; set; }

        /// <summary>
        /// 所有的存储空间（以字节为单位）
        /// </summary>
        public long AllStorage { get; set; }
    }
}
