using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.WorkoutExercise
{
    public class WorkoutExerciseDto
    {
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }

        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? WeightUsed { get; set; }
    }
}
