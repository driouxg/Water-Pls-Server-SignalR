using SignalRTest.Domain;
using SignalRTest.Domain.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
