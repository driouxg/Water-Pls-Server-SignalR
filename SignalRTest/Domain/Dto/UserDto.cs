namespace SignalRTest.Domain.Dto
{
    public class UserDto
    {
        public UserDto(int Id, string Username)
        {
            this.Id = Id;
            this.Username = Username;
        }
        public int Id { get; set; }
        public string Username { get; set; }
    }
}