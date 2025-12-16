using FitnessTracker.Application.DTOs.Auth;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserDto dto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Register request for username {Username}", dto.Username);

            var result = await _authService.RegisterAsync(dto, cancellationToken);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for username {Username}", dto.Username);

            var result = await _authService.LoginAsync(dto, cancellationToken);

            return Ok(result);
        }
    }
}
