/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    使用supersocket实现的一个server基类
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace Cares.Fids.Monitor.Models.Model
{
    /// <summary>
    /// 使用supersocket实现的server基类
    /// </summary>
    public class BaseServer:AppServer<BaseSession>
    {
        /// <summary>
        /// 以此来确认tcp的请求
        /// </summary>
        public const String RequestTerminator = Constants.DEFAULT_TERMINATOR;
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseServer():base(new TerminatorReceiveFilterFactory(RequestTerminator))
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="terminator">终止符</param>
        public BaseServer(String terminator)
            : base(new TerminatorReceiveFilterFactory(terminator))
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
        protected override void OnStopped()
        {
            base.OnStopped();
        }
    }
}
