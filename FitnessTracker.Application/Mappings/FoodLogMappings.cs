using FitnessTracker.Application.DTOs.FoodLog;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Mappings
{
    public static class FoodLogMappings
    {
        public static FoodLogDto ToDto(this FoodLog entity)
        {
            return new FoodLogDto
            {
                Id = entity.Id,
                LogDate = entity.LogDate,
                Servings = entity.Servings,
                Quantity = entity.Quantity,
                UserId = entity.UserId,
                FoodId = entity.FoodId
            };
        }

        public static FoodLog ToEntity(this CreateFoodLogDto dto)
        {
            return new FoodLog
            {
                LogDate = dto.LogDate,
                Servings = dto.Servings,
                Quantity = dto.Quantity,
                UserId = dto.UserId,
                FoodId = dto.FoodId
            };
        }

        public static void UpdateEntity(this FoodLog entity, UpdateFoodLogDto dto)
        {
            entity.LogDate = dto.LogDate;
            entity.Servings = dto.Servings;
            entity.Quantity = dto.Quantity;
        }
    }
}

