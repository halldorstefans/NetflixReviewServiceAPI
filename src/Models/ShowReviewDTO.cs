using System.ComponentModel.DataAnnotations;

namespace NetflixReviewService.Models
{
    public class ShowReviewDTO
    {                
        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }
    }
}