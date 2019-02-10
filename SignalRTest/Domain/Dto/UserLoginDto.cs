using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Dto
{
    public class UserLoginDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public bool rememberMe { get; set; }
    }
}
