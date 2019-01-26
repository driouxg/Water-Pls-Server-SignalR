using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRTest.Domain.Dto;

namespace SignalRTest.Domain.VO
{
    public class GeoCoordinatesVO : ValueObject
    {
        public GeoCoordinateVO latitude { get; }
        public GeoCoordinateVO longitude { get; }

        public GeoCoordinatesVO(GeoCoordinateVO latitude, GeoCoordinateVO longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double GetLatitude()
        {
            return latitude.value;
        }

        public double GetLongitude()
        {
            return longitude.value;
        }
    }
}
