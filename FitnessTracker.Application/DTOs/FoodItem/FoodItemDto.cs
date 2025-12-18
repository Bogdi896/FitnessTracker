using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.FoodItem
{
    public class FoodItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
    }
}
