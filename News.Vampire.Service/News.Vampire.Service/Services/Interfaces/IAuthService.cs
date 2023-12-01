using News.Vampire.Service.Models.UserManagement;

namespace News.Vampire.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> Registration(RegistrationDto model);
        Task<AuthServiceResponseDto> Login(LoginDto model);
        Task<AuthServiceResponseDto> AddToRole(UpdatePermissionDto updatePermissionDto);
    }
}
