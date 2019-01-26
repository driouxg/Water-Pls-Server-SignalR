using System;
using System.Collections.Generic;
using System.Text;
using SignalRTest.Domain.Dto;
using SignalRTest.Domain.VO;
using Xunit;

namespace Water_Pls_Server_SignalR_Tests
{
    public class LocationDistanceCalculatorTest
    {


        [Fact]
        public void CalculateDistance()
        {
            // Arrange
            GeoCoordinateVO latitude = new GeoCoordinateVO(123.23424);
            GeoCoordinateVO longitude = new GeoCoordinateVO(23.234324);
            GeoCoordinatesVO coordinates = new GeoCoordinatesVO(latitude, longitude);

            // Act
            var result = coordinates.

            // Assert

        }
    }
}
