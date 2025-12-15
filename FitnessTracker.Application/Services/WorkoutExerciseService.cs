using FitnessTracker.Application.DTOs.WorkoutExercise;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Services
{
    public class WorkoutExerciseService : IWorkoutExerciseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutExerciseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WorkoutExerciseDto>> GetByWorkoutAsync(int workoutId, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.WorkoutExercises.FindAsync(we => we.WorkoutId == workoutId);
            return items.Select(we => we.ToDto());
        }

        public async Task<WorkoutExerciseDto?> GetAsync(int workoutId, int exerciseId, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.WorkoutExercises.FindAsync(we =>
                we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            var entity = items.FirstOrDefault();
            return entity?.ToDto();
        }

        public async Task<WorkoutExerciseDto> CreateAsync(CreateWorkoutExerciseDto dto, CancellationToken cancellationToken)
        {
            ValidateWorkoutExercise(dto.Sets, dto.Reps, dto.WeightUsed);

            var workout = await _unitOfWork.Workouts.GetByIdAsync(dto.WorkoutId);
            if (workout == null)
                throw new ArgumentException("Workout not found.", nameof(dto.WorkoutId));

            var exercise = await _unitOfWork.Exercises.GetByIdAsync(dto.ExerciseId);
            if (exercise == null)
                throw new ArgumentException("Exercise not found.", nameof(dto.ExerciseId));

            var existing = await _unitOfWork.WorkoutExercises.FindAsync(we =>
                we.WorkoutId == dto.WorkoutId && we.ExerciseId == dto.ExerciseId);

            if (existing.Any())
                throw new ArgumentException("This workout already contains this exercise.", nameof(dto.ExerciseId));

            var entity = dto.ToEntity();
            await _unitOfWork.WorkoutExercises.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<WorkoutExerciseDto?> UpdateAsync(int workoutId, int exerciseId, UpdateWorkoutExerciseDto dto, CancellationToken cancellationToken)
        {
            ValidateWorkoutExercise(dto.Sets, dto.Reps, dto.WeightUsed);

            var items = await _unitOfWork.WorkoutExercises.FindAsync(we =>
                we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            var entity = items.FirstOrDefault();
            if (entity == null)
                return null;

            entity.UpdateEntity(dto);
            _unitOfWork.WorkoutExercises.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<bool> DeleteAsync(int workoutId, int exerciseId, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.WorkoutExercises.FindAsync(we =>
                we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            var entity = items.FirstOrDefault();
            if (entity == null)
                return false;

            _unitOfWork.WorkoutExercises.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static void ValidateWorkoutExercise(int sets, int reps, decimal? weightUsed)
        {
            if (sets <= 0)
                throw new ArgumentException("Sets must be a positive value.", nameof(sets));

            if (reps <= 0)
                throw new ArgumentException("Reps must be a positive value.", nameof(reps));

            if (weightUsed.HasValue && weightUsed.Value < 0)
                throw new ArgumentException("Weight used cannot be negative.", nameof(weightUsed));
        }
    }
}
