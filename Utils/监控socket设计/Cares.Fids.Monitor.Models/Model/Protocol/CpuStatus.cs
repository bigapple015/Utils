/**
 * author:  cmlu
 * date:    2015年1月8日
 * desc:    cpu状况
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Model.Protocol
{
    /// <summary>
    /// cpu状况
    /// </summary>
    public class CpuStatus
    {
        /// <summary>
        /// cpu核心数
        /// </summary>
        public int CpuCore;

        /// <summary>
        /// cpu使用占比
        /// </summary>
        public double UsedPercent;
    }
}
