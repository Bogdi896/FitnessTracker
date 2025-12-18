using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.Workout
{
    public class CreateWorkoutDto
    {
        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int DurationInMinutes { get; set; }

        public string? Notes { get; set; }
    }
}
