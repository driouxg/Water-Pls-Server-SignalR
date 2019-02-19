using System.ComponentModel.DataAnnotations;

namespace SignalRTest.Domain.Dto
{
    public class UserRegistrationDto : Entity.Entity
    {
        public int Id { get; protected set; }
        [Required]
        public string username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
