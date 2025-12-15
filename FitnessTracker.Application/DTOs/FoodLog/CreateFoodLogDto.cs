using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.FoodLog
{
    public class CreateFoodLogDto
    {
        public DateTime LogDate { get; set; }

        public decimal Servings { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }

        public int FoodId { get; set; }
    }
}
