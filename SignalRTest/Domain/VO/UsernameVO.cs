using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.VO
{
    public class UsernameVo : ValueObject
    {
        public string value { get; }

        public UsernameVo(string value)
        {
            value = value;
            isAlphanumeric();
        }

        private void isAlphanumeric()
        {
            if (value.All(x => char.IsLetterOrDigit(x)))
            {
                throw new ArgumentException($"Username: '{value}' must be alphanumeric.");
            }
        }
    }
}
