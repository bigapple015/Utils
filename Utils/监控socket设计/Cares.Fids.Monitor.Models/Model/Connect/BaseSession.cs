/**
 * author:  cmlu
 * date:    2015年1月7日
 * desc:    实现一个基类的session，代表与客户端的回话
 * 
 */

using System;
using SuperSocket.SocketBase;

namespace Cares.Fids.Monitor.Models.Model.Connect
{
    /// <summary>
    /// 实现一个基类的session，代表与客户端的回话
    /// </summary>
    public class BaseSession:AppSession<BaseSession>
    {
        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }

        protected override void HandleException(Exception e)
        {

        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }
    }
}
