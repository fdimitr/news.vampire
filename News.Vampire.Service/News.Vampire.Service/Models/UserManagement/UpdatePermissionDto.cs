using System.ComponentModel.DataAnnotations;

namespace News.Vampire.Service.Models.UserManagement
{
    public class UpdatePermissionDto : LoginDto
    {
        [Required(ErrorMessage = "Identifier is required")]
        public required string Identifier { get; set; }
    }
}
