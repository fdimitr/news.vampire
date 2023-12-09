using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using News.Vampire.Service.Services.Interfaces;
using News.Vampire.Service.Models.UserManagement;

namespace News.Vampire.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        internal static string GetFullLogin(LoginDto loginDto) => $"{loginDto.Username}-{loginDto.UniqueDeviceId}";

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            _configuration = configuration;

        }
        public async Task<AuthServiceResponseDto> Registration(RegistrationDto registrationDto)
        {
            var userExists = await _userManager.FindByNameAsync(registrationDto.Username);
            if (userExists != null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists"
                };

            ApplicationUser newUser = new()
            {
                UserName = GetFullLogin(registrationDto),
                Email = registrationDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registrationDto.UniqueDeviceId);
            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += $" # {error.Description}{Environment.NewLine}";
                }
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            // Add a Default USER Role to all users
            string role = registrationDto.Identifier == _configuration["JWT:Prerogative"] ? StaticUserRoles.Admin : StaticUserRoles.User;
            await _userManager.AddToRoleAsync(newUser, role);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User Created Successfully"
            };
        }

        public async Task<AuthServiceResponseDto> Login(LoginDto loginDto)
        {
            if (loginDto.Username is null || loginDto.UniqueDeviceId is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Incorrect Credentials"
                };

            var user = await _userManager.FindByNameAsync(GetFullLogin(loginDto));

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.UniqueDeviceId);

            if (!isPasswordCorrect)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = token
            };
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
            var tokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWT:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<AuthServiceResponseDto> AddToRole(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(GetFullLogin(updatePermissionDto));

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid User!"
                };

            string role = updatePermissionDto.Identifier == _configuration["JWT:Prerogative"] ? StaticUserRoles.Admin : StaticUserRoles.User;
            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
            };
        }
    }
}
