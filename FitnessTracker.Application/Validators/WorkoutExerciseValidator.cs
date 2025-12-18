using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Validators
{
    public static class WorkoutExerciseValidator
    {
        public static void ValidateWorkoutExercise(int sets, int reps, decimal? weightUsed)
        {
            if (sets <= 0)
                throw new ArgumentException("Sets must be a positive value.", nameof(sets));

            if (reps <= 0)
                throw new ArgumentException("Reps must be a positive value.", nameof(reps));

            if (weightUsed.HasValue && weightUsed.Value < 0)
                throw new ArgumentException("Weight used cannot be negative.", nameof(weightUsed));
        }
    }
}
