﻿namespace SignalRTest.Domain.Dto
{
    public class AddressDto : Entity.Entity
    {
        public int streetNumber { get; set; }
        public string streetName { get; set; }
        public string route { get; set; }
        public string cityName { get; set; }
        public string stateName { get; set; }
        public int zipcode { get; set; }
    }
}
