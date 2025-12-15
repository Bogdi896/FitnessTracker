using FitnessTracker.Application.DTOs.Goal;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Mappings
{
    public static class GoalMappings
    {
        public static GoalDto ToDto(this Goal entity)
        {
            return new GoalDto
            {
                Id = entity.Id,
                GoalType = entity.GoalType,
                TargetValue = entity.TargetValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsAchieved = entity.IsAchieved,
                UserId = entity.UserId
            };
        }

        public static Goal ToEntity(this CreateGoalDto dto)
        {
            return new Goal
            {
                GoalType = dto.GoalType,
                TargetValue = dto.TargetValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = dto.UserId,
                IsAchieved = false
            };
        }

        public static void UpdateEntity(this Goal entity, UpdateGoalDto dto)
        {
            entity.GoalType = dto.GoalType;
            entity.TargetValue = dto.TargetValue;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.IsAchieved = dto.IsAchieved;
        }
    }
}

