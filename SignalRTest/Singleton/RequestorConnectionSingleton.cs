using SignalRTest.Domain;
using SignalRTest.Domain.VO;
using System;

namespace SignalRTest.Singleton
{
    public class RequestorConnectionSingleton
    {
        private static readonly Lazy<ConnectionMap<Domain.VO.ApplicationUser>> lazy =
        new Lazy<ConnectionMap<Domain.VO.ApplicationUser>>(() => new ConnectionMap<Domain.VO.ApplicationUser>());

        public static ConnectionMap<Domain.VO.ApplicationUser> Instance { get { return lazy.Value; } }

        private RequestorConnectionSingleton()
        {
        }
    }
}
