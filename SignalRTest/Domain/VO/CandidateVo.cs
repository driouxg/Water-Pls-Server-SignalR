using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.VO
{
    public class CandidateVo : ValueObject
    {
        public UsernameVo username { get; }
        public GeoCoordinatesVo GeoCoordinates { get; }

        public CandidateVo(UsernameVo username, GeoCoordinatesVo geoCoordinates)
        {
            this.username = username;
            this.GeoCoordinates = geoCoordinates;
        }

        public CandidateVo(string username, GeoCoordinatesVo geoCoordinates)
        {
            this.username = new UsernameVo(username);
            this.GeoCoordinates = geoCoordinates;
        }
    }
}
