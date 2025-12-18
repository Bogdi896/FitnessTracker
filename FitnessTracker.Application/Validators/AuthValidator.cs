using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.Auth;
using FitnessTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Application.Validators
{
    public static class AuthValidator
    {
        public static async Task ValidateRegisterDto(
            RegisterUserDto dto,
            IUnitOfWork unitOfWork,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            UserValidator.ValidateUser(dto);
            var existingByUsername = await unitOfWork.Users.FindAsync(u => u.Username == dto.Username);
            if (existingByUsername.Any())
            {
                logger.LogWarning("Registration failed: username {Username} already exists.", dto.Username);
                throw new ArgumentException("Username already exists.", nameof(dto.Username));
            }

            var existingByEmail = await unitOfWork.Users.FindAsync(u => u.Email == dto.Email);
            if (existingByEmail.Any())
            {
                logger.LogWarning("Registration failed: email {Email} already exists.", dto.Email);
                throw new ArgumentException("Email already exists.", nameof(dto.Email));
            }

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.", nameof(dto.Password));
            }
        }
    }
}
