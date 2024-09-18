using Haiku.API.Utility;
using System.ComponentModel.DataAnnotations;

namespace Haiku.API.Models
{
    public class HaikuItem
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100.")]
        public string Title { get; set; } = "Untitled";

        [SyllableCount(5, ErrorMessage = "Must be five syllables")]
        [Required]
        public string LineOne { get; set; }

        [SyllableCount(7, ErrorMessage = "Must be seven syllables")]
        [Required]
        public string LineTwo { get; set; }

        [SyllableCount(5, ErrorMessage = "Must be five syllables")]
        [Required]
        public string LineThree { get; set; }

        public long CreatorId { get; set; } = 1;

    }
}

