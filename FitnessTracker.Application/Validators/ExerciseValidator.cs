using FitnessTracker.Application.DTOs.Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class ExerciseValidator
    {
        private static readonly string[] AllowedMuscleGroups =
            { "back", "chest", "arms", "shoulders", "legs", "abdomen" };

        private static readonly string[] AllowedDifficultyLevels =
            { "beginner", "intermediate", "advanced" };

        public static void ValidateExercise(CreateExerciseDto dto)
        {
            ValidateCommon(dto.Name, dto.MuscleGroup, dto.DifficultyLevel);
        }

        public static void ValidateExercise(UpdateExerciseDto dto)
        {
            ValidateCommon(dto.Name, dto.MuscleGroup, dto.DifficultyLevel);
        }

        private static void ValidateCommon(string name, string muscleGroup, string difficultyLevel)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Name must be at most 100 characters.", nameof(name));

            if (string.IsNullOrWhiteSpace(muscleGroup))
                throw new ArgumentException("Muscle group is required.", nameof(muscleGroup));

            var mg = muscleGroup.Trim().ToLowerInvariant();
            if (!AllowedMuscleGroups.Contains(mg))
                throw new ArgumentException(
                    $"Muscle group must be one of: {string.Join(", ", AllowedMuscleGroups)}.",
                    nameof(muscleGroup));

            if (string.IsNullOrWhiteSpace(difficultyLevel))
                throw new ArgumentException("Difficulty level is required.", nameof(difficultyLevel));

            var diff = difficultyLevel.Trim().ToLowerInvariant();
            if (!AllowedDifficultyLevels.Contains(diff))
                throw new ArgumentException(
                    $"Difficulty level must be one of: {string.Join(", ", AllowedDifficultyLevels)}.",
                    nameof(difficultyLevel));
        }
    }
}
