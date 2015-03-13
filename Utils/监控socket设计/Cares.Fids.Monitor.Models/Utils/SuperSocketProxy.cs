using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Model;
using Cares.Fids.Monitor.Models.Model.Connect;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;

namespace Cares.Fids.Monitor.Models.Utils
{
    public static class SuperSocketProxy
    {
        public static IBootstrap bootstrap = BootstrapFactory.CreateBootstrap();
        public static ClientServer clientServer;

        /// <summary>
        /// 在指定接口开始服务监听
        /// </summary>
        /// <param name="port"></param>
        public static void Start()
        {
            
            if (!bootstrap.Initialize())
            {
                return;
            }
            /*
            if (bootstrap.Start() != StartResult.Success)
            {
                return;
            }
            
            clientServer = (ClientServer) bootstrap.GetServerByName("clientServer");
            clientServer.NewRequestReceived += delegate(BaseSession session, StringRequestInfo info)
            {
                LogUtils.Info("新的请求：" + info.Body);
                session.Send("");
            };
            clientServer.NewSessionConnected += delegate(BaseSession session)
            {
               
            };*/
            clientServer = new ClientServer();
            ServerConfig serverConfig = new ServerConfig();
            serverConfig.Ip = "Any";
            serverConfig.Port = 23356;
            if (!clientServer.Setup(serverConfig,new SocketServerFactory(),new TerminatorReceiveFilterFactory(Constants.DEFAULT_TERMINATOR),new Log4NetLogFactory("log4net.config")))
            {
                LogUtils.Error("启动设置失败");
                return;
            }

            if (!clientServer.Start())
            {
                LogUtils.Error("启动失败");
                return;
            }

            /*

            AppServer appServer = new AppServer();

            var serverConfig = new ServerConfig();

            //ip: 服务器监听的ip地址。你可以设置具体的地址，也可以设置为下面的值 Any 
            //serverConfig.Ip = "184.56.41.24";

            serverConfig.Port = 8848;

            if (!appServer.Setup(serverConfig))
            { //Setup with listening IP and port//启动失败
                Console.WriteLine("Failed to setup!");
                return;
            }

            //   appServer.NewSessionConnected += new SessionHandler<GPSSession>(mainClass.appServer_NewSessionConnected);
            //   appServer.NewRequestReceived += new RequestHandler<GPSSession, BinaryRequestInfo>(mainClass.appServer_NewRequestReceived);
            //   appServer.SessionClosed += new SessionHandler<GPSSession, SuperSocket.SocketBase.CloseReason>(mainClass.OnSocketError);

            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                return;
            }
            Console.WriteLine("ssssss前置机启动成功！,输入q关闭服务器");


            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();

                continue;


            }

            appServer.Stop();
            Console.WriteLine("ss服务器已经关闭");
            Console.ReadKey();*/
        }

    }
}
 