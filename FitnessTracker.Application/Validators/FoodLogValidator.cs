using FitnessTracker.Application.DTOs.FoodLog;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class FoodLogValidator
    {
        public static void Validate(CreateFoodLogDto dto, User user)
        {
            ValidateCommon(dto.LogDate, dto.Servings, dto.Quantity, user);
        }

        public static void Validate(UpdateFoodLogDto dto, User user)
        {
            ValidateCommon(dto.LogDate, dto.Servings, dto.Quantity, user);
        }

        private static void ValidateCommon(DateTime logDate, decimal servings, int quantity, User user)
        {
            if (logDate > DateTime.UtcNow)
                throw new ArgumentException("Log date cannot be in the future.", nameof(logDate));

            if (logDate < user.RegistrationDate)
                throw new ArgumentException("Food log cannot occur before user registration date.", nameof(logDate));

            if (servings <= 0)
                throw new ArgumentException("Servings must be greater than zero.", nameof(servings));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }
    }
}
