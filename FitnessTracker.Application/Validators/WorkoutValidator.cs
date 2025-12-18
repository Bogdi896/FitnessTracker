using FitnessTracker.Application.DTOs.Workout;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class WorkoutValidator
    {
        public static void ValidateWorkout(CreateWorkoutDto dto, User user)
        {
            ValidateCommon(dto.Date, dto.DurationInMinutes, user);
        }

        public static void ValidateWorkout(UpdateWorkoutDto dto, User user)
        {
            ValidateCommon(dto.Date, dto.DurationInMinutes, user);
        }

        private static void ValidateCommon(DateTime date, int durationMinutes, User user)
        {
            var now = DateTime.UtcNow;
            if (date > now)
                throw new ArgumentException("Workout date cannot be in the future.", nameof(date));

            if (durationMinutes <= 0)
                throw new ArgumentException("Duration must be positive.", nameof(durationMinutes));

            if (durationMinutes < 5 || durationMinutes > 300)
                throw new ArgumentException("Duration must be between 5 and 300 minutes.", nameof(durationMinutes));

            if (date < user.RegistrationDate)
                throw new ArgumentException("Workout date cannot be before user registration date.", nameof(date));
        }
    }
}
