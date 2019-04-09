using System;
using SignalRTest.Domain.VO;

namespace SignalRTest.Domain.Dto
{
    public class GeoCoordinateVo : ValueObject
    {
        public float value { get; }

        public GeoCoordinateVo(float value)
        {
            this.value = value;
            validateGeoCoordinate();
        }

        private void validateGeoCoordinate()
        {
            if (value > 180 || value < -180)
            {
                throw new ArgumentException($"GeoCoordinate: '{value}' must be in the range [-180, 180]");
            }
        }
    }
}
