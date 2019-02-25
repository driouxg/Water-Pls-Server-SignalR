using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Entity
{
    public class GeoCoordinatesEntity : IEntity
    {
        public int Id { get; protected set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
