using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Offer { get; set; }
        [Required, MaxLength(50)]
        public string ButtonText { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
