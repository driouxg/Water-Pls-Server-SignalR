using SignalRTest.Domain;
using System;
using SignalRTest.Domain.VO;

namespace SignalRTest.Singleton
{
    public class RequestorConnectionSingleton
    {
        private static readonly Lazy<ConnectionMap<UsernameVo>> lazy =
        new Lazy<ConnectionMap<UsernameVo>>(() => new ConnectionMap<UsernameVo>());

        public static ConnectionMap<UsernameVo> Instance { get { return lazy.Value; } }

        private RequestorConnectionSingleton()
        {
        }
    }
}
