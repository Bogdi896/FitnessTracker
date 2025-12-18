using FitnessTracker.Application.DTOs.Auth;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Security;
using FitnessTracker.Application.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FitnessTracker.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IConfiguration configuration, ILogger<AuthService> logger, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterUserDto dto,
            CancellationToken cancellationToken)
        {
            await AuthValidator.ValidateRegisterDto(dto, _unitOfWork, _logger, cancellationToken);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Height = dto.Height,
                Weight = dto.Weight,
                RegistrationDate = DateTime.UtcNow,

                Username = dto.Username,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Registered new user {UserId} with username {Username}", user.Id, user.Username);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token.TokenString,
                ExpiresAt = token.ExpiresAt,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto,
            CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.Username == dto.Username);
            var user = users.FirstOrDefault();
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for username {Username}", dto.Username);
                throw new ArgumentException("Invalid username or password.");
            }
            _logger.LogInformation("User {UserId} logged in successfully.", user.Id);
            var token = GenerateJwtToken(user);
            return new AuthResponseDto
            {
                Token = token.TokenString,
                ExpiresAt = token.ExpiresAt,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        private (string TokenString, DateTime ExpiresAt) GenerateJwtToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var secretKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(1);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, expires);
        }
    }
}

