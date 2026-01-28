using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code must have at least 3 characters")]
        [MaxLength(5, ErrorMessage = "Code must have at most 5 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Code must have at most 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
