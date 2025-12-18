using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.Workout;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Repositories;
using FitnessTracker.Application.Validators;

namespace FitnessTracker.Application.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WorkoutDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var workouts = await _unitOfWork.Workouts.GetAllAsync();
            return workouts.Select(w => w.ToDto());
        }

        public async Task<IEnumerable<WorkoutDto>> GetByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var workouts = await _unitOfWork.Workouts.FindAsync(w => w.UserId == userId);
            return workouts.Select(w => w.ToDto());
        }

        public async Task<WorkoutDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var workout = await _unitOfWork.Workouts.GetByIdAsync(id);
            return workout?.ToDto();
        }

        public async Task<WorkoutDto> CreateAsync(CreateWorkoutDto dto, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new ArgumentException("User not found.", nameof(dto.UserId));

            WorkoutValidator.ValidateWorkout(dto, user);

            var duplicates = await _unitOfWork.Workouts.FindAsync(w =>
                w.UserId == dto.UserId &&
                w.Date.Year == dto.Date.Year &&
                w.Date.Month == dto.Date.Month &&
                w.Date.Day == dto.Date.Day &&
                w.Date.Hour == dto.Date.Hour &&
                w.Date.Minute == dto.Date.Minute
            );

            if (duplicates.Any())
                throw new ArgumentException("User already has a workout at this start time (to the minute).", nameof(dto.Date));

            var entity = dto.ToEntity();
            await _unitOfWork.Workouts.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<WorkoutDto?> UpdateAsync(int id, UpdateWorkoutDto dto, CancellationToken cancellationToken)
        {
            var workout = await _unitOfWork.Workouts.GetByIdAsync(id);
            if (workout == null)
                return null;

            var user = await _unitOfWork.Users.GetByIdAsync(workout.UserId);
            if (user == null)
                throw new InvalidOperationException("Workout's user no longer exists.");

            WorkoutValidator.ValidateWorkout(dto, user);

            var duplicates = await _unitOfWork.Workouts.FindAsync(w =>
                w.UserId == workout.UserId &&
                w.Id != workout.Id &&
                w.Date.Year == dto.Date.Year &&
                w.Date.Month == dto.Date.Month &&
                w.Date.Day == dto.Date.Day &&
                w.Date.Hour == dto.Date.Hour &&
                w.Date.Minute == dto.Date.Minute
            );

            if (duplicates.Any())
                throw new ArgumentException("User already has a workout at this start time (to the minute).", nameof(dto.Date));

            workout.UpdateEntity(dto);
            _unitOfWork.Workouts.Update(workout);
            await _unitOfWork.SaveChangesAsync();

            return workout.ToDto();
        }
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var workout = await _unitOfWork.Workouts.GetByIdAsync(id);
            if (workout == null)
                return false;

            _unitOfWork.Workouts.Delete(workout);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

