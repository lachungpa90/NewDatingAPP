using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class RegisterDtos
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}