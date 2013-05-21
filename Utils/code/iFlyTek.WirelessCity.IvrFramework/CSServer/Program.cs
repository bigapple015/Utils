using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NLogHandler;
using System.Threading;
using NLog;
using Utility;

namespace CSServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = (int)SocketFlags.None;
            Console.WriteLine(i);
            Logger logger = LogManager.GetLogger("console");
            string key = DESHelper.GenerateKey();
            string str = DESHelper.MD5Encrypt("我要查询一下2012年6月12日的双色球", "12345678");
            string str2 = DESHelper.MD5Decrypt(str, "12345678");
            logger.Info(str);
            logger.Info(str2);
        }
    }
}
