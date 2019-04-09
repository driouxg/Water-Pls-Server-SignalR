using System;
namespace SignalRTest.Domain.Dto
{
    public class CandidateDto
    {
        public string username { get; set; }
        public GeoCoordinatesDto GeoCoordinatesDto { get; set; }

        public CandidateDto(string username, GeoCoordinatesDto geoCoordinatesDto)
        {
            this.username = username;
            this.GeoCoordinatesDto = geoCoordinatesDto;
        }
    }
}