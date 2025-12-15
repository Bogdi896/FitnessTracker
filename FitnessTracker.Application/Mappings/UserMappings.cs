using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.User;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Application.Mappings
{
    public static class UserMappings
    {
        public static UserDto ToDto(this User entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                BirthDate = entity.BirthDate,
                Gender = entity.Gender,
                Height = entity.Height,
                Weight = entity.Weight,
                RegistrationDate = entity.RegistrationDate
            };
        }

        public static void UpdateEntity(this User entity, UpdateUserDto dto)
        {
            entity.Name = dto.Name;
            entity.Email = dto.Email;
            entity.BirthDate = dto.BirthDate;
            entity.Gender = dto.Gender;
            entity.Height = dto.Height;
            entity.Weight = dto.Weight;
        }

        public static User ToEntity(this CreateUserDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Height = dto.Height,
                Weight = dto.Weight,
                RegistrationDate = DateTime.UtcNow
            };
        }
    }
}