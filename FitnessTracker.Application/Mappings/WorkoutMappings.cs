using FitnessTracker.Application.DTOs.Workout;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Mappings
{
    public static class WorkoutMappings
    {
        public static WorkoutDto ToDto(this Workout entity)
        {
            return new WorkoutDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Date = entity.Date,
                DurationInMinutes = entity.DurationInMinutes,
                Notes = entity.Notes
            };
        }

        public static Workout ToEntity(this CreateWorkoutDto dto)
        {
            return new Workout
            {
                UserId = dto.UserId,
                Date = dto.Date,
                DurationInMinutes = dto.DurationInMinutes,
                Notes = dto.Notes
            };
        }

        public static void UpdateEntity(this Workout entity, UpdateWorkoutDto dto)
        {
            entity.Date = dto.Date;
            entity.DurationInMinutes = dto.DurationInMinutes;
            entity.Notes = dto.Notes;
        }
    }
}

