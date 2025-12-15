using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.Exercise
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string MuscleGroup { get; set; } = null!;
        public string DifficultyLevel { get; set; } = null!;
    }
}
