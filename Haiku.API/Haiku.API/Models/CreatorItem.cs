using System.ComponentModel.DataAnnotations;

namespace Haiku.API.Models
{
    public class CreatorItem
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }


        [StringLength(1000, ErrorMessage = "Bio length can't be more than 1000.")]
        public string? Bio { get; set; } = "No bio.";
    }
}
