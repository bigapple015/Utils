using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cares.Fids.Monitor.Models.Utils
{
    public class OneRunner
    {
        /// <summary>
        /// IsOnlyOneProcessRunning
        /// </summary>
        /// <returns></returns>
        public static bool IsOnlyOneProcessRunning()
        {
            Process[] runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            return runningProcesses.Length == 1;
        }
    }
}
