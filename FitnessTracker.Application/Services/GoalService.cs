using FitnessTracker.Application.DTOs.Goal;
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
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;

        private static readonly string[] AllowedGoalTypes =
        {
            "weight_loss",
            "weight_gain",
            "distance",
            "duration"
        };

        public GoalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GoalDto>> GetByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var goals = await _unitOfWork.Goals.FindAsync(g => g.UserId == userId);
            return goals.Select(g => g.ToDto());
        }

        public async Task<GoalDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            return goal?.ToDto();
        }

        public async Task<GoalDto> CreateAsync(CreateGoalDto dto, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId)
                ?? throw new ArgumentException("User not found.", nameof(dto.UserId));

            ValidateGoal(dto, user);

            await EnsureNoOverlap(dto.UserId, dto.GoalType, dto.StartDate, dto.EndDate);

            var entity = dto.ToEntity();
            await _unitOfWork.Goals.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<GoalDto?> UpdateAsync(int id, UpdateGoalDto dto, CancellationToken cancellationToken)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return null;

            var user = await _unitOfWork.Users.GetByIdAsync(goal.UserId)
                ?? throw new ArgumentException("User not found.", nameof(goal.UserId));

            ValidateGoal(dto, user);

            await EnsureNoOverlap(goal.UserId, dto.GoalType, dto.StartDate, dto.EndDate, excludeGoalId: id);

            goal.UpdateEntity(dto);
            _unitOfWork.Goals.Update(goal);
            await _unitOfWork.SaveChangesAsync();

            return goal.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return false;

            _unitOfWork.Goals.Delete(goal);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


        private void ValidateGoal(CreateGoalDto dto, User user)
        {
            ValidateCommon(dto.GoalType, dto.TargetValue, dto.StartDate, dto.EndDate, user);
        }

        private void ValidateGoal(UpdateGoalDto dto, User user)
        {
            ValidateCommon(dto.GoalType, dto.TargetValue, dto.StartDate, dto.EndDate, user);
        }

        private void ValidateCommon(
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

        private async Task EnsureNoOverlap(
            int userId,
            string goalType,
            DateOnly start,
            DateOnly end,
            int? excludeGoalId = null)
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

