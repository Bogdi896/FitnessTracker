using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.WorkoutExercise
{
    public class UpdateWorkoutExerciseDto
    {
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? WeightUsed { get; set; }
    }

}
