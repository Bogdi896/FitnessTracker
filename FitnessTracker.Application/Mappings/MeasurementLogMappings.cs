using FitnessTracker.Application.DTOs.MeasurementLog;
using FitnessTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Mappings
{
    public static class MeasurementLogMappings
    {
        public static MeasurementLogDto ToDto(this MeasurementLog entity)
        {
            return new MeasurementLogDto
            {
                Id = entity.Id,
                Date = entity.Date,
                Weight = entity.Weight,
                BodyFatPercentage = entity.BodyFatPercentage,
                WaistCircumference = entity.WaistCircumference,
                Chest = entity.Chest,
                Arms = entity.Arms,
                UserId = entity.UserId
            };
        }

        public static MeasurementLog ToEntity(this CreateMeasurementLogDto dto)
        {
            return new MeasurementLog
            {
                Date = dto.Date,
                Weight = dto.Weight,
                BodyFatPercentage = dto.BodyFatPercentage,
                WaistCircumference = dto.WaistCircumference,
                Chest = dto.Chest,
                Arms = dto.Arms,
                UserId = dto.UserId
            };
        }

        public static void UpdateEntity(this MeasurementLog entity, UpdateMeasurementLogDto dto)
        {
            entity.Date = dto.Date;
            entity.Weight = dto.Weight;
            entity.BodyFatPercentage = dto.BodyFatPercentage;
            entity.WaistCircumference = dto.WaistCircumference;
            entity.Chest = dto.Chest;
            entity.Arms = dto.Arms;
        }
    }
}
