using System.ComponentModel.DataAnnotations;

namespace CargoManagementAPi.Models
{
    public class CargoStatus
    {
        [Key]
        public int StatusId { get; set; }
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }
    }
}
