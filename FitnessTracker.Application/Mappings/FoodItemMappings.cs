using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.FoodItem;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Application.Mappings
{
    public static class FoodItemMappings
    {
        public static FoodItemDto ToDto(this FoodItem entity)
        {
            return new FoodItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Calories = entity.Calories,
                Protein = entity.Protein,
                Carbs = entity.Carbs,
                Fat = entity.Fat
            };
        }

        public static FoodItem ToEntity(this CreateFoodItemDto dto)
        {
            return new FoodItem
            {
                Name = dto.Name,
                Calories = dto.Calories,
                Protein = dto.Protein,
                Carbs = dto.Carbs,
                Fat = dto.Fat
            };
        }

        public static void UpdateEntity(this FoodItem entity, UpdateFoodItemDto dto)
        {
            entity.Name = dto.Name;
            entity.Calories = dto.Calories;
            entity.Protein = dto.Protein;
            entity.Carbs = dto.Carbs;
            entity.Fat = dto.Fat;
        }
    }
}
