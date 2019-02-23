using SignalRTest.Domain.Dto;

namespace SignalRTest.Domain.VO
{
    public class UserVo : ValueObject
    {
        public UsernameVo username { get; }
        public NameVo firstName { get; }
        public NameVo lastName { get; }
        public EmailVo email { get; }
        public AddressDto addressDto { get; }
        public GeoCoordinatesVo geoCoordinates { get; }
        public string clientConnection { get; }

        public UserVo(UsernameVo username, NameVo firstName, NameVo lastName, EmailVo email, AddressDto addressDto, GeoCoordinatesVo geoCoordinates, string clientConnection)
        {
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.addressDto = addressDto;
            this.geoCoordinates = geoCoordinates;
            this.clientConnection = clientConnection;
        }
    }
}
