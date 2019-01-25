using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.VO
{
    public class UsernameVO : ValueObject
    {
        public string value { get; }

        public UsernameVO(string value)
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
