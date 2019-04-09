using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Dto
{
    public class SendEmailResponse
    {
        public bool Successful => !(Errors?.Count > 0);
        public List<string> Errors { get; set; }
    }
}
