using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NLogHandler;
using System.Threading;

namespace CSServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string str = client.DownloadString("http://blog.csdn.net/a497785609/article/details/5941283");
            NLogHandler.NlogHelper.Debug(str);
        }
    }
}
