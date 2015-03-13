/**
 * author：  cmlu
 * date：    2015年1月8日
 * desc：    内存信息
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryStatus
    {
        /// <summary>
        /// 使用的内存（以byte为单位）
        /// </summary>
        public long UsedMemory { get; set; }

        /// <summary>
        /// 所有的内存（以byte为单位）
        /// </summary>
        public long AllMemory { get; set; }
    }
}
