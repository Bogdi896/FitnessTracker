using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class MeasurementLogValidator
    {
        private const decimal MaxDailyWeightChangeKg = 3m;
        public static void ValidateValues(
           DateOnly date,
           decimal weight,
           decimal? bodyFatPercentage,
           decimal? waist,
           decimal? chest,
           decimal? arms)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (date > today)
                throw new ArgumentException("Measurement date cannot be in the future.", nameof(date));

            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero.", nameof(weight));

            if (bodyFatPercentage.HasValue)
            {
                if (bodyFatPercentage.Value < 2 || bodyFatPercentage.Value > 60)
                    throw new ArgumentException("Body fat percentage must be between 2% and 60%.", nameof(bodyFatPercentage));
            }

            if (waist.HasValue)
            {
                if (waist.Value <= 0 || waist.Value < 30 || waist.Value > 200)
                    throw new ArgumentException("Waist circumference must be between 30 and 200 cm.", nameof(waist));
            }

            if (chest.HasValue)
            {
                if (chest.Value <= 0 || chest.Value < 50 || chest.Value > 200)
                    throw new ArgumentException("Chest circumference must be between 50 and 200 cm.", nameof(chest));
            }

            if (arms.HasValue)
            {
                if (arms.Value <= 0 || arms.Value < 15 || arms.Value > 80)
                    throw new ArgumentException("Arm circumference must be between 15 and 80 cm.", nameof(arms));
            }
        }

        public static void ValidateWeightChange(DateOnly newDate, decimal newWeight, List<MeasurementLog> existingLogs)
        {
            if (!existingLogs.Any())
                return;

            foreach (var log in existingLogs)
            {
                var daysDiff = Math.Abs((newDate.ToDateTime(TimeOnly.MinValue) - log.Date.ToDateTime(TimeOnly.MinValue)).TotalDays);

                if (daysDiff <= 0)
                {
                    daysDiff = 1;
                }

                var allowedChange = MaxDailyWeightChangeKg * (decimal)daysDiff;
                var actualChange = Math.Abs(newWeight - log.Weight);

                if (actualChange > allowedChange)
                {
                    throw new ArgumentException(
                        $"Weight change is too large: {actualChange:0.##} kg over {daysDiff:0.##} day(s). " +
                        $"Maximum allowed is {allowedChange:0.##} kg.",
                        nameof(newWeight));
                }
            }
        }
    }
}
