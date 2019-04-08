using SignalRTest.Domain.Dto;

namespace SignalRTest.Domain.VO
{
    public class GeoCoordinatesVo : ValueObject
    {
        public GeoCoordinateVo latitude { get; }
        public GeoCoordinateVo longitude { get; }

        public GeoCoordinatesVo(GeoCoordinateVo latitude, GeoCoordinateVo longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public GeoCoordinatesVo(float latitude, float longitude)
        {
            this.latitude = new GeoCoordinateVo(latitude);
            this.longitude = new GeoCoordinateVo(longitude);
        }

        public GeoCoordinatesVo(GeoCoordinatesDto geoCoordinatesDto)
        {
            latitude = new GeoCoordinateVo(geoCoordinatesDto.latitude);
            longitude = new GeoCoordinateVo(geoCoordinatesDto.longitude);
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
