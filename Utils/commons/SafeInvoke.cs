using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scmis.Plc.Utils
{
    /// <summary>
    /// 安全调用
    /// </summary>
    public static class SafeInvoke
    {
        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        public static void Safe(this Action action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
        }
    }
}
