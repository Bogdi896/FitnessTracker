using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.Goal
{
    public class CreateGoalDto
    {
        public string GoalType { get; set; } = null!;
        public decimal TargetValue { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int UserId { get; set; }
    }
}
