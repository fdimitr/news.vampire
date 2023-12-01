using Microsoft.AspNetCore.Identity;

namespace News.Vampire.Service.Models.UserManagement
{
    public class ApplicationUser: IdentityUser
    {
        public string? OsVersion { get; set; }

        public string? DeviceName { get; set; }
    }
}
