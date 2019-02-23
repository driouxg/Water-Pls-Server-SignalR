using SignalRTest.Domain;
using System;

namespace SignalRTest.Singleton
{
    public class RequestorConnectionSingleton
    {
        private static readonly Lazy<ConnectionMap<ApplicationUser>> lazy =
        new Lazy<ConnectionMap<ApplicationUser>>(() => new ConnectionMap<ApplicationUser>());

        public static ConnectionMap<ApplicationUser> Instance { get { return lazy.Value; } }

        private RequestorConnectionSingleton()
        {
        }
    }
}
