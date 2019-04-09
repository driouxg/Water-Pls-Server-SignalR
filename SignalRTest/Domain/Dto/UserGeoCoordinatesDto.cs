using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Dto
{
    public class UserGeoCoordinatesDto
    {
        public UsernameDto Username { get; set; }
        public GeoCoordinatesDto GeoCoordinates { get; set; }
    }
}
