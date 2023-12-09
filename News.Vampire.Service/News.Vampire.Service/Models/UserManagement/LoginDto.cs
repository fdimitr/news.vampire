using System.ComponentModel.DataAnnotations;

namespace News.Vampire.Service.Models.UserManagement
{
    public class LoginDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Identification ID is required")]
        public required string UniqueDeviceId { get; set; }
    }
}
