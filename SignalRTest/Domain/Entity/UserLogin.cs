using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SignalRTest.Domain.Entity
{
    public class UserLogin : IEntity
    {
        public int Id { get; protected set; }
        public IdentityUserLogin<string> IdentityUserLogin { get; set; }
    }
}
