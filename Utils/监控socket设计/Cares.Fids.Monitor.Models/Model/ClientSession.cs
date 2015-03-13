using System;
using SuperSocket.SocketBase;

namespace Cares.Fids.Monitor.Models.Model
{
    public class ClientSession:AppSession<ClientSession>
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
