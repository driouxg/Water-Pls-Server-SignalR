using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SignalRTest.Domain.VO
{
    public class EmailVO : ValueObject
    {
        public string value { get; }

        public EmailVO(string value)
        {
            this.value = value;
            validateEmail();
        }

        private void validateEmail()
        {
            try
            {
                MailAddress eMailAddress = new MailAddress(value);
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Email: '{value}' is not a valid email address.");
            }
        }
    }
}
