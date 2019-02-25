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
            GeoCoordinateVo latitude1 = new GeoCoordinateVo(51.5f);
            GeoCoordinateVo longitude1 = new GeoCoordinateVo(0);
            GeoCoordinatesVo coordinates1 = new GeoCoordinatesVo(latitude1, longitude1);

            GeoCoordinateVo latitude2 = new GeoCoordinateVo(38.8f);
            GeoCoordinateVo longitude2 = new GeoCoordinateVo(-77.1f);
            GeoCoordinatesVo coordinates2 = new GeoCoordinatesVo(latitude2, longitude2);

            LocationDistanceCalculator calculator = new LocationDistanceCalculator(coordinates1, coordinates2);

            // Act
            var result = Math.Round(calculator.CalculateDistanceInMeters(), 2);

            // Assert
            Assert.Equal(5918185.06, result);
        }
    }
}
