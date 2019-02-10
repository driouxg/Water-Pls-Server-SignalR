using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Entity
{
    public class Entity : IEntity
    {
        public int Id { get; protected set; }
    }
}
