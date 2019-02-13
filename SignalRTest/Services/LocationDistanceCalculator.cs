using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SignalRTest.Domain.VO;

namespace SignalRTest.Services
{
    public class LocationDistanceCalculator
    {
        private ILogger _logger;
        private readonly GeoCoordinatesVo location1;
        private readonly GeoCoordinatesVo location2;

        public LocationDistanceCalculator(GeoCoordinatesVo location1, GeoCoordinatesVo location2)
        {
            this.location1 = location1;
            this.location2 = location2;
        }

        public double CalculateDistanceInMeters()
        {
            int radiusOfEarthInMeters = 6371000;
            var lat1 = DegreesToRadians(location1.GetLatitude());
            var lat2 = DegreesToRadians(location2.GetLatitude());

            var lon1 = DegreesToRadians(location1.GetLongitude());
            var lon2 = DegreesToRadians(location2.GetLongitude());

            var d_lat = lat2 - lat1;
            var d_lon = lon2 - lon1;

            var a = Math.Pow(Math.Sin(d_lat / 2.0), 2) + Math.Pow(Math.Sin(d_lon / 2), 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return radiusOfEarthInMeters * c;
        }

        public double CalculateDistanceInMiles()
        {
            return CalculateDistanceInMeters() / 0.000621371;
        }

        protected double DegreesToRadians(double value)
        {
            return value * Math.PI / 180;
        }
    }
}
