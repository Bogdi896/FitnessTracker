using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("FoodItem")]
public class FoodItem
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public int Calories { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Protein { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Carbs { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Fat { get; set; }

    [InverseProperty("Food")]
    public virtual ICollection<FoodLog> FoodLogs { get; set; } = new List<FoodLog>();
}
