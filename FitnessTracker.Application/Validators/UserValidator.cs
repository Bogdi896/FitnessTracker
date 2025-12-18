using FitnessTracker.Application.DTOs.Auth;
using FitnessTracker.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class UserValidator
    {
        public static void ValidateUser(RegisterUserDto dto)
        {
            ValidateCommon(
                dto.Name,
                dto.Email,
                dto.BirthDate,
                dto.Gender,
                dto.Height,
                dto.Weight
            );
        }
        public static void ValidateUser(CreateUserDto dto)
        {
            ValidateCommon(
                dto.Name,
                dto.Email,
                dto.BirthDate,
                dto.Gender,
                dto.Height,
                dto.Weight
            );
        }
        public static void ValidateUser(UpdateUserDto dto)
        {
            ValidateCommon(
                dto.Name,
                dto.Email,
                dto.BirthDate,
                dto.Gender,
                dto.Height,
                dto.Weight
            );
        }

        private static void ValidateCommon(
            string name,
            string email,
            DateOnly birthDate,
            string gender,
            decimal height,
            decimal weight)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Length > 50)
                throw new ArgumentException("Name must be at most 50 characters.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));
            if (!IsValidEmail(email))
                throw new ArgumentException("Email format is invalid.", nameof(email));

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (birthDate >= today)
                throw new ArgumentException("Birth date must be in the past.", nameof(birthDate));


            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;

            if (age < 13)
                throw new ArgumentException("User must be at least 13 years old.", nameof(birthDate));

            var normalizedGender = gender?.Trim().ToLowerInvariant();
            var allowedGenders = new[] { "male", "female" };
            if (!allowedGenders.Contains(normalizedGender))
                throw new ArgumentException("Gender must be either 'male' or 'female'.", nameof(gender));

            if (height < 50 || height > 272) // in cm
                throw new ArgumentException("Height must be between 50 and 272 cm.", nameof(height));

            if (weight < 20 || weight > 400) // in kg
                throw new ArgumentException("Weight must be between 20 and 400 kg.", nameof(weight));
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
