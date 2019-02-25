using Microsoft.AspNetCore.Identity;
using SignalRTest.Domain.Entity;

namespace SignalRTest.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public GeoCoordinatesEntity geoCoordinates { get; set; }
    }
}
