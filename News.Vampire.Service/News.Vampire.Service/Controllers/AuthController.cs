using Microsoft.AspNetCore.Mvc;
using News.Vampire.Service.Models.UserManagement;
using News.Vampire.Service.Services.Interfaces;

namespace News.Vampire.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
        {
            var registerResult = await _authService.Registration(registerDto);

            return registerResult.IsSucceed ? Ok(registerResult) : BadRequest(registerResult);
        }


        // Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.Login(loginDto);

            return loginResult.IsSucceed ? Ok(loginResult) : Unauthorized(loginResult);
        }



        // Route -> make user -> admin
        [HttpPost]
        [Route("add-to-role")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.AddToRole(updatePermissionDto);

            return operationResult.IsSucceed ? Ok(operationResult) : BadRequest(operationResult);
        }
    }
}
