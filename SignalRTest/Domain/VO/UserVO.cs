using SignalRTest.Domain.Dto;

namespace SignalRTest.Domain.VO
{
    public class UserVO : ValueObject
    {
        public UsernameVO username { get; }
        public NameVO firstName { get; }
        public NameVO lastName { get; }
        public EmailVO email { get; }
        public AddressDto addressDto { get; }
        public GeoCoordinatesVO geoCoordinates { get; }
        public string clientConnection { get; }

        public UserVO(UsernameVO username, NameVO firstName, NameVO lastName, EmailVO email, AddressDto addressDto, GeoCoordinatesVO geoCoordinates, string clientConnection)
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
