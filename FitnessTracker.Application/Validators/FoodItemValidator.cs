using FitnessTracker.Application.DTOs.FoodItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class FoodItemValidator
    {
        public static void Validate(CreateFoodItemDto dto)
        {
            ValidateCommon(dto.Name, dto.Calories, dto.Protein, dto.Carbs, dto.Fat);
        }

        public static void Validate(UpdateFoodItemDto dto)
        {
            ValidateCommon(dto.Name, dto.Calories, dto.Protein, dto.Carbs, dto.Fat);
        }

        private static void ValidateCommon(string name, int calories, decimal protein, decimal carbs, decimal fat)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Name must be at most 100 characters.", nameof(name));
            if (calories < 0)
                throw new ArgumentException("Calories cannot be negative.", nameof(calories));
            if (calories > 1200)
                throw new ArgumentException("Calories per 100g are unrealistically high.", nameof(calories));
            if (protein < 0 || protein > 100)
                throw new ArgumentException("Protein per 100g must be between 0 and 100g.", nameof(protein));
            if (carbs < 0 || carbs > 100)
                throw new ArgumentException("Carbs per 100g must be between 0 and 100g.", nameof(carbs));
            if (fat < 0 || fat > 100)
                throw new ArgumentException("Fat per 100g must be between 0 and 100g.", nameof(fat));
            if (protein + carbs + fat > 100)
                throw new ArgumentException("Protein + Carbs + Fat per 100g cannot exceed 100g total.");
        }
    }
}
