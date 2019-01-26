using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.VO
{
    public class NameVO : ValueObject
    {
        public string value { get; }

        public NameVO(string value)
        {
            this.value = value;
            isAlpha();
        }

        private void isAlpha()
        {
            if (!value.All(x => char.IsLetter(x)))
            {
                throw new ArgumentException($"Name: '{value}' must only contain letters A-Z");
            }
        }
    }
}
