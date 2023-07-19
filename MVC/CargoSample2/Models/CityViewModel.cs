using System.ComponentModel.DataAnnotations;

namespace CargoSample2.Models
{
    public class CityViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CityName { get; set; }
        [Required]
        [MaxLength(6)]
        public int Pincode { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }

    }
}
