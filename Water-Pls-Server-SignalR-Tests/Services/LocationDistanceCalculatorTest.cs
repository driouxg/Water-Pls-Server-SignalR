using System;
using System.Collections.Generic;
using System.Text;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.VO;
using SignalRTest.Services;
using Xunit;

namespace Water_Pls_Server_SignalR_Tests
{
    public class LocationDistanceCalculatorTest
    {
        [Fact]
        public void CalculateDistance()
        {
            // Arrange
            GeoCoordinateVO latitude1 = new GeoCoordinateVO(51.5);
            GeoCoordinateVO longitude1 = new GeoCoordinateVO(0);
            GeoCoordinatesVO coordinates1 = new GeoCoordinatesVO(latitude1, longitude1);

            GeoCoordinateVO latitude2 = new GeoCoordinateVO(38.8);
            GeoCoordinateVO longitude2 = new GeoCoordinateVO(-77.1);
            GeoCoordinatesVO coordinates2 = new GeoCoordinatesVO(latitude2, longitude2);

            LocationDistanceCalculator calculator = new LocationDistanceCalculator(coordinates1, coordinates2);

            // Act
            var result = Math.Round(calculator.CalculateDistanceInMeters(), 2);

            // Assert
            Assert.Equal(5918185.06, result);
        }
    }
}
