using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.MeasurementLog
{
    public class MeasurementLogDto
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public decimal Weight { get; set; }

        public decimal? BodyFatPercentage { get; set; }

        public decimal? WaistCircumference { get; set; }

        public decimal? Chest { get; set; }

        public decimal? Arms { get; set; }

        public int UserId { get; set; }
    }
}
