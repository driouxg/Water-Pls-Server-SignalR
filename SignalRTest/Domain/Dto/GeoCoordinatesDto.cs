namespace SignalRTest.Domain.Dto
{
    public class GeoCoordinatesDto : Entity.IEntity
    {
        public int Id { get; protected set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
