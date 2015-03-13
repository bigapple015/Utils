/**
 */

using System;
using SuperSocket.SocketBase.Config;

namespace Cares.Fids.Monitor.Models.Model.Connect
{
    /// <summary>
    /// 客户端server
    /// </summary>
    public class ClientServer:BaseServer
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientServer()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="terminator">终止符</param>
        public ClientServer(String terminator)
            : base(terminator)
        {

        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnNewSessionConnected(BaseSession session)
        {
            base.OnNewSessionConnected(session);
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        
    }
}
