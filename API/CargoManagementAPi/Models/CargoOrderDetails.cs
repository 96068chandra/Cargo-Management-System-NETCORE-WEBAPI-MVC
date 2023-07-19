using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace CargoManagementAPi.Models
{
    public class CargoOrderDetails
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
        public CargoStatus CargoStatus { get; set; }
        public int CargoStatusId { get; set; }

        [ForeignKey("Customer")]
        public int CustId { get; set; }

        //[ForeignKey("Cargo")]
        //public int CargoId { get; set; }

        public CargoType CargoType { get; set; }

        public int CargoTypeId { get; set; }

        public City City { get; set; }

        public int CityId { get; set; }

        public double Weight { get; set; }

        public double Price { get; set; }






    }
}
