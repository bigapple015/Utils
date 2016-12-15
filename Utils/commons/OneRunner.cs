using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scmis.Plc.Utils
{
    /// <summary>
    /// 单实例运行
    /// </summary>
    public static class OneRunner
    {

        /// <summary>
        /// 应用程序的ID
        /// </summary>
        private const string APP_ID = @"ee1218a379ef4c66aeae78ef836c6f7b";

        /// <summary>
        /// 用于测试单实例的Mutex
        /// </summary>
        private static Mutex OneRunMutex = null;

        /// <summary>
        /// 是否之前已经运行了实例
        /// 此方法只能程序运行时调用一次
        /// </summary>
        /// <returns></returns>
        public static bool IsAlreadyRun()
        {
            if (OneRunMutex != null)
            {
                throw new ArgumentException("这个方法仅能调用一次");
            }

            bool createdNew = false;
            //注意退出时释放Mutex
            OneRunMutex = new Mutex(true, APP_ID, out createdNew);

            if (!createdNew)
            {
                //已经运行了一个实例
                //此时不需要释放Mutex，因为你没有拥有锁
                return true;
            }
            else
            {
                //第一次运行，程序退出前释放锁，实际上不释放也没关系，因为退出程序会自动释放所有资源  包括锁
                return false;
            }
        }

        /// <summary>
        /// 释放锁
        /// 放到程序退出的最后一行代码
        /// </summary>
        public static void ReleaseMutex()
        {
            if (OneRunMutex != null)
            {
                OneRunMutex.ReleaseMutex();
                OneRunMutex = null;
            }
        }
    }
}
