using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace CargoSample2.Models
{
    public class CargoOrderViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]

        [DataType(DataType.DateTime)]

        public DateTime OrderDate { get; set; }

        [Required]

        public string OrderId { get; set; }
        [Required]
        public string ReceiverName { get; set; }

        public string ReceiverEmail { get; set; }
        public string ReceiverPhoneNo { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsAccepted { get; set; }
        public List<CargoStatusViewModel> CargoStatus { get; set; }
        public int CargoStatusId { get; set; }

        [ForeignKey("Customer")]
        public int CustId { get; set; }

        //[ForeignKey("Cargo")]
        //public int CargoId { get; set; }

        public List<CargoType> CargoType { get; set; }

        public int CargoTypeId { get; set; }

        public List<CityViewModel> City { get; set; }

        public int CityId { get; set; }

        public double Weight { get; set; }

        public double Price { get; set; }


    }
}
