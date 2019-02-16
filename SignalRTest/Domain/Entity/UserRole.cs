using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Entity
{
    public class UserRole : IEntity
    {
        public string Role { get; set; }
    }
}
