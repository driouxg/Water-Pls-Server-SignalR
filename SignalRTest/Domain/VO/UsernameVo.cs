using System;
using System.Linq;

namespace SignalRTest.Domain.VO
{
    public class UsernameVo : ValueObject
    {
        public string _value { get; }

        public UsernameVo(string value)
        {
            _value = value;
            IsAlphaNumeric();
        }

        private void IsAlphaNumeric()
        {
            if (!_value.All(x => char.IsLetterOrDigit(x)))
            {
                throw new ArgumentException($"Name: '{_value}' must only contain letters A-Z or digits 0-9");
            }
        }
    }
}
