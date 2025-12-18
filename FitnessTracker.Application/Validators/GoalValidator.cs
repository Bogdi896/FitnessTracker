using FitnessTracker.Application.DTOs.Goal;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class GoalValidator
    {
        private static readonly string[] AllowedGoalTypes =
        {
            "weight_loss",
            "weight_gain",
            "distance",
            "duration"
        };

        public static void ValidateGoal(CreateGoalDto dto, User user)
        {
            ValidateCommon(dto.GoalType, dto.TargetValue, dto.StartDate, dto.EndDate, user);
        }

        public static void ValidateGoal(UpdateGoalDto dto, User user)
        {
            ValidateCommon(dto.GoalType, dto.TargetValue, dto.StartDate, dto.EndDate, user);
        }

        private static void ValidateCommon(
            string goalType,
            decimal targetValue,
            DateOnly startDate,
            DateOnly endDate,
            User user)
        {
            if (!AllowedGoalTypes.Contains(goalType.ToLower()))
                throw new ArgumentException(
                    $"GoalType must be one of: {string.Join(", ", AllowedGoalTypes)}",
                    nameof(goalType));

            if (targetValue <= 0)
                throw new ArgumentException("TargetValue must be positive.", nameof(targetValue));

            if (startDate > endDate)
                throw new ArgumentException("StartDate must be <= EndDate.", nameof(startDate));

            if (startDate < DateOnly.FromDateTime(user.RegistrationDate))
                throw new ArgumentException("StartDate cannot be before user's registration date.", nameof(startDate));

            var lowerType = goalType.ToLower();

            if (lowerType == "weight_loss" && targetValue >= user.Weight)
                throw new ArgumentException("Weight-loss goal requires TargetValue < current weight.");

            if (lowerType == "weight_gain" && targetValue <= user.Weight)
                throw new ArgumentException("Weight-gain goal requires TargetValue > current weight.");
        }

        public static async Task EnsureNoOverlap(
            int userId,
            string goalType,
            IUnitOfWork _unitOfWork,
            DateOnly start,
            DateOnly end,
            int? excludeGoalId = null
            )
        {
            var goals = await _unitOfWork.Goals.FindAsync(g =>
                g.UserId == userId &&
                g.GoalType.ToLower() == goalType.ToLower());

            foreach (var g in goals)
            {
                if (excludeGoalId.HasValue && g.Id == excludeGoalId.Value)
                    continue;

                if (start <= g.EndDate && end >= g.StartDate)
                {
                    throw new ArgumentException(
                        "User already has a goal of this type that overlaps with this date range."
                    );
                }
            }
        }
    }
}
