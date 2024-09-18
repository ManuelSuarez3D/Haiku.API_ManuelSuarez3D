using Haiku.API.Utility;
using System.ComponentModel.DataAnnotations;

namespace Haiku.API.Dtos
{
    public class HaikuDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100.")]
        public string Title { get; set; } = "Untitled";

        [SyllableCount(5, ErrorMessage = "Must be five syllables")]
        [Required]
        [StringLength(50, ErrorMessage = "First line length can't be more than 50.")]
        public string LineOne { get; set; }

        [SyllableCount(7, ErrorMessage = "Must be seven syllables")]
        [Required]
        [StringLength(50, ErrorMessage = "Second line length can't be more than 50.")]
        public string LineTwo { get; set; }

        [SyllableCount(5, ErrorMessage = "Must be five syllables")]
        [Required]
        [StringLength(50, ErrorMessage = "Third line length can't be more than 50.")]
        public string LineThree { get; set; }

        public long CreatorId { get; set; }
    }
}
