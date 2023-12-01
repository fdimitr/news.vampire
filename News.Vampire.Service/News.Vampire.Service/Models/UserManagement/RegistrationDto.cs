using System.ComponentModel.DataAnnotations;

namespace News.Vampire.Service.Models.UserManagement
{
    public class RegistrationDto : LoginDto
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? OsVersion { get; set; }

        public string? DeviceName { get; set; }

        public string? Identifier { get; set; }
    }
}
