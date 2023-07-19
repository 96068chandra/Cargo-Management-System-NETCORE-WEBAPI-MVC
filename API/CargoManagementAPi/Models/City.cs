using System.ComponentModel.DataAnnotations;

namespace CargoManagementAPi.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CityName { get; set; }
        [Required]
        [MaxLength(7)]
        public int Pincode { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        

    }

}
