using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace CargoManagementAPi.Models
{
    public class Cargo
    {
        [Key]
        public int CargoId { get; set; }
        [Required]
        public string CargoName { get; set; }
        [Required]
        public string Place { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Required]
        public double Price { get; set; }


        [Required]
        public double Weight { get; set; }

        public CargoType CargoType { get; set; }
        public int CargoTypeId { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //today or future date is valid
            if (OrderDate <= DateTime.Today)
            {
                yield return new ValidationResult("Date of Order shouldn't be in the past",
                    new string[] { nameof(OrderDate) });
            }

        }
    }
}
