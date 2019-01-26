using System;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.VO;

namespace SignalRTest.Services
{
    public class LocationDistanceCalculator
    {
        private readonly GeoCoordinatesVO location1;
        private readonly GeoCoordinatesVO location2;

        public LocationDistanceCalculator(GeoCoordinatesVO location1, GeoCoordinatesVO location2)
        {
            this.location1 = location1;
            this.location2 = location2;
        }

        public double CalculateDistanceInMeters()
        {
            int radiusOfEarthInMeters = 6371000;
            var phi_1 = ConvertToRadians(location1.GetLatitude());
            var phi_2 = ConvertToRadians(location2.GetLatitude());

            var delta_phi = ConvertToRadians(location2.GetLatitude() - location1.GetLatitude());
            var delta_lambda = ConvertToRadians(location2.GetLongitude() - location1.GetLongitude());

            var a = Math.Pow(Math.Sin(delta_phi / 2.0), 2) + Math.Cos(phi_1) + Math.Cos(phi_2) + Math.Pow(Math.Sin(delta_lambda / 2.0), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return radiusOfEarthInMeters * c;
        }

        public double CalculateDistanceInMiles()
        {
            return CalculateDistanceInMeters() / 0.000621371;
        }

        public double ConvertToRadians(double value)
        {
            return (Math.PI * 180) / value;
        }
    }
}
