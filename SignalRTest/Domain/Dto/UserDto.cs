namespace SignalRTest.Domain.Dto
{
    public class UserDto : Entity.Entity
    {
        public string Username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public AddressDto addressDto { get; set; }
        public GeoCoordinatesDto geoCoordinatesDto { get; set; }
        public string clientConnection { get; set; }
        public string connectionStatus { get; set; }
    }
}