using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SignalRTest.Services;

namespace SignalRTest.Domain.VO
{
    public class ConnectionStatusVo : ValueObject
    {
        public string value { get; }

        public ConnectionStatusVo(string value)
        {
            this.value = value;
            ValidateStatus();
        }

        private void ValidateStatus()
        {
            EnumUtility.stringValueOf(value, ConnectionEnum);
        }
    }

    enum ConnectionEnum
    {
        [DescriptionAttribute("Reconnecting")] Reconnecting,
        [DescriptionAttribute("Connected")] Connected,
        [DescriptionAttribute("Disconnected")] Disconnected
    }
}
