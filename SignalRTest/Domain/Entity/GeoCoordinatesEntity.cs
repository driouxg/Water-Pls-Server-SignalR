namespace SignalRTest.Domain.Entity
{
    public class GeoCoordinatesEntity : IEntity
    {
        public int Id { get; protected set; }
        public string Username { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}
