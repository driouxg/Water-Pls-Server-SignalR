using SignalRTest.Domain;
using SignalRTest.Domain.VO;
using System;

namespace SignalRTest.Singleton
{
    public class DonatorConnectionSingleton
    {
        private static readonly Lazy<ConnectionMap<UsernameVo>> lazy =
        new Lazy<ConnectionMap<UsernameVo>>(() => new ConnectionMap<UsernameVo>());

        public static ConnectionMap<UsernameVo> Instance { get { return lazy.Value; } }

        private DonatorConnectionSingleton()
        {
        }
    }
}
