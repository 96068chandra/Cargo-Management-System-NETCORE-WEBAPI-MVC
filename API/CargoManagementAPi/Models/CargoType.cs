using System.ComponentModel.DataAnnotations;

namespace CargoManagementAPi.Models
{
    public class CargoType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        //Weight in Kgs
        public double Weight { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        //Original price
        public double Price { get; set; }
        public double ExtraWeight { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        //Extra charges for additional weight
        public double ExtraPrice { get; set; }
    }
}
