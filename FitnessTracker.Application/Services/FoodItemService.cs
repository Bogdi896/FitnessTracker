using FitnessTracker.Application.DTOs.FoodItem;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FoodItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.FoodItems.GetAllAsync();
            return items.Select(i => i.ToDto());
        }

        public async Task<FoodItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.FoodItems.GetByIdAsync(id);
            return item?.ToDto();
        }

        public async Task<FoodItemDto> CreateAsync(CreateFoodItemDto dto, CancellationToken cancellationToken)
        {
            Validate(dto);
            var existing = await _unitOfWork.FoodItems.FindAsync(f =>
                f.Name.ToLower() == dto.Name.ToLower());

            if (existing.Any())
                throw new ArgumentException("A food item with this name already exists.", nameof(dto.Name));

            var entity = dto.ToEntity();
            await _unitOfWork.FoodItems.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<FoodItemDto?> UpdateAsync(int id, UpdateFoodItemDto dto, CancellationToken cancellationToken)
        {
            Validate(dto);

            var item = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (item == null)
                return null;

            var existing = await _unitOfWork.FoodItems.FindAsync(f =>
                f.Id != id && f.Name.ToLower() == dto.Name.ToLower());

            if (existing.Any())
                throw new ArgumentException("A food item with this name already exists.", nameof(dto.Name));

            item.UpdateEntity(dto);
            _unitOfWork.FoodItems.Update(item);
            await _unitOfWork.SaveChangesAsync();

            return item.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (item == null)
                return false;

            var logs = await _unitOfWork.FoodLogs.FindAsync(fl => fl.FoodId == id);
            if (logs.Any())
            {
                throw new InvalidOperationException("Cannot delete food item that is referenced by food logs.");
            }

            _unitOfWork.FoodItems.Delete(item);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static void Validate(CreateFoodItemDto dto)
        {
            ValidateCommon(dto.Name, dto.Calories, dto.Protein, dto.Carbs, dto.Fat);
        }

        private static void Validate(UpdateFoodItemDto dto)
        {
            ValidateCommon(dto.Name, dto.Calories, dto.Protein, dto.Carbs, dto.Fat);
        }

        private static void ValidateCommon(string name, int calories, decimal protein, decimal carbs, decimal fat)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Name must be at most 100 characters.", nameof(name));
            if (calories < 0)
                throw new ArgumentException("Calories cannot be negative.", nameof(calories));
            if (calories > 1200)
                throw new ArgumentException("Calories per 100g are unrealistically high.", nameof(calories));
            if (protein < 0 || protein > 100)
                throw new ArgumentException("Protein per 100g must be between 0 and 100g.", nameof(protein));
            if (carbs < 0 || carbs > 100)
                throw new ArgumentException("Carbs per 100g must be between 0 and 100g.", nameof(carbs));
            if (fat < 0 || fat > 100)
                throw new ArgumentException("Fat per 100g must be between 0 and 100g.", nameof(fat));
            if (protein + carbs + fat > 100)
                throw new ArgumentException("Protein + Carbs + Fat per 100g cannot exceed 100g total.");
        }
    }
}
