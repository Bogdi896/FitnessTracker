using FitnessTracker.Application.DTOs.WorkoutExercise;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Mappings
{
    public static class WorkoutExerciseMappings
    {
        public static WorkoutExerciseDto ToDto(this WorkoutExercise entity)
        {
            return new WorkoutExerciseDto
            {
                WorkoutId = entity.WorkoutId,
                ExerciseId = entity.ExerciseId,
                Sets = entity.Sets,
                Reps = entity.Reps,
                WeightUsed = entity.WeightUsed
            };
        }

        public static WorkoutExercise ToEntity(this CreateWorkoutExerciseDto dto)
        {
            return new WorkoutExercise
            {
                WorkoutId = dto.WorkoutId,
                ExerciseId = dto.ExerciseId,
                Sets = dto.Sets,
                Reps = dto.Reps,
                WeightUsed = dto.WeightUsed
            };
        }

        public static void UpdateEntity(this WorkoutExercise entity, UpdateWorkoutExerciseDto dto)
        {
            entity.Sets = dto.Sets;
            entity.Reps = dto.Reps;
            entity.WeightUsed = dto.WeightUsed;
        }
    }
}

