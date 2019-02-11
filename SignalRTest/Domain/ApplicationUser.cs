using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRTest.Domain.Dto;

namespace SignalRTest.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public UserLoginDto userLogin {get; set; }
    }
}
