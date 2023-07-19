using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace CargoSample2.Models
{
    public class CargoViewModel
    {
        [Required]
        public int CargoId { get; set; }
        [Required]
        public string CargoName { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public double Price { get; set; }
        public double Weight { get; set; }
        public List<CargoType> cargotype { get; set; }
        public int Id { get; set; }

    }
}
