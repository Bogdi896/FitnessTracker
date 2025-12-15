using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.Exercise;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Repositories;

namespace FitnessTracker.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IUnitOfWork _unitOfWork;

        private static readonly string[] AllowedMuscleGroups =
            { "back", "chest", "arms", "shoulders", "legs", "abdomen" };

        private static readonly string[] AllowedDifficultyLevels =
            { "beginner", "intermediate", "advanced" };

        public ExerciseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExerciseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var exercises = await _unitOfWork.Exercises.GetAllAsync();
            return exercises.Select(e => e.ToDto());
        }

        public async Task<ExerciseDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var exercise = await _unitOfWork.Exercises.GetByIdAsync(id);
            return exercise?.ToDto();
        }

        public async Task<ExerciseDto> CreateAsync(CreateExerciseDto dto, CancellationToken cancellationToken)
        {
            ValidateExercise(dto);

            var existing = await _unitOfWork.Exercises.FindAsync(e =>
                e.Name.ToLower() == dto.Name.ToLower());

            if (existing.Any())
                throw new ArgumentException("Exercise name must be unique.", nameof(dto.Name));

            var entity = dto.ToEntity();
            await _unitOfWork.Exercises.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<ExerciseDto?> UpdateAsync(int id, UpdateExerciseDto dto, CancellationToken cancellationToken)
        {
            ValidateExercise(dto);

            var exercise = await _unitOfWork.Exercises.GetByIdAsync(id);
            if (exercise == null)
                return null;

            var existing = await _unitOfWork.Exercises.FindAsync(e =>
                e.Id != id && e.Name.ToLower() == dto.Name.ToLower());

            if (existing.Any())
                throw new ArgumentException("Exercise name must be unique.", nameof(dto.Name));

            exercise.UpdateEntity(dto);
            _unitOfWork.Exercises.Update(exercise);
            await _unitOfWork.SaveChangesAsync();

            return exercise.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var exercise = await _unitOfWork.Exercises.GetByIdAsync(id);
            if (exercise == null)
                return false;

            var workoutExercises = await _unitOfWork.WorkoutExercises.FindAsync(we => we.ExerciseId == id);

            if (workoutExercises.Any())
            {
                var workoutIds = workoutExercises.Select(we => we.WorkoutId).Distinct().ToList();

                var workouts = await _unitOfWork.Workouts.FindAsync(w => workoutIds.Contains(w.Id));

                var now = DateTime.UtcNow;

                if (workouts.Any(w => w.Date <= now))
                {
                    throw new InvalidOperationException(
                        "Exercise cannot be deleted because it is used in previous workouts.");
                }
            }

            _unitOfWork.Exercises.Delete(exercise);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static void ValidateExercise(CreateExerciseDto dto)
        {
            ValidateCommon(dto.Name, dto.MuscleGroup, dto.DifficultyLevel);
        }

        private static void ValidateExercise(UpdateExerciseDto dto)
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
