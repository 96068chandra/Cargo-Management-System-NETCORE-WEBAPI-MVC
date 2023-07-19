using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CargoManagementAPi.Models
{
    public class Admin:AdminLoginModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        public string? Name { get; set; }
        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DefaultValue("Admin@123")]

        public string Password { get; set; }
    }

    public class AdminLoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DefaultValue("Admin@123")]

        public string Password { get; set; }
    }
}
