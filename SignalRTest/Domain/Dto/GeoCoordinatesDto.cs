namespace SignalRTest.Domain.Dto
{
    public class GeoCoordinatesDto
    {
        public float latitude { get; set; }
        public float longitude { get; set; }

        public GeoCoordinatesDto(float latitude, float longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}
