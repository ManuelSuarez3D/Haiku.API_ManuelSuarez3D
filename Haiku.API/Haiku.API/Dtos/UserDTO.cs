using System.ComponentModel.DataAnnotations;

namespace Haiku.API.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
