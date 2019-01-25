using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.VO
{
    public class UsernameVO : ValueObject
    {
        private string _value { get; }

        public UsernameVO(string value)
        {
            _value = value;
            isAlphanumeric();
        }

        private void isAlphanumeric()
        {
            if (_value.All(x => char.IsLetterOrDigit(x)))
            {
                throw new ArgumentException($"Username: '{_value}' must be alphanumeric.");
            }
        }
    }
}
