using FitnessTracker.Application.DTOs.Exercise;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Mappings
{
    public static class ExerciseMappings
    {
        public static ExerciseDto ToDto(this Exercise entity)
        {
            return new ExerciseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                MuscleGroup = entity.MuscleGroup,
                DifficultyLevel = entity.DifficultyLevel
            };
        }

        public static Exercise ToEntity(this CreateExerciseDto dto)
        {
            return new Exercise
            {
                Name = dto.Name,
                MuscleGroup = dto.MuscleGroup,
                DifficultyLevel = dto.DifficultyLevel
            };
        }

        public static void UpdateEntity(this Exercise entity, UpdateExerciseDto dto)
        {
            entity.Name = dto.Name;
            entity.MuscleGroup = dto.MuscleGroup;
            entity.DifficultyLevel = dto.DifficultyLevel;
        }
    }
}

