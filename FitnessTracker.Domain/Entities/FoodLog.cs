using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("FoodLog")]
public partial class FoodLog
{
    [Key]
    public int Id { get; set; }

    public DateTime LogDate { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Servings { get; set; }

    public int Quantity { get; set; }

    public int UserId { get; set; }

    public int FoodId { get; set; }

    [ForeignKey("FoodId")]
    [InverseProperty("FoodLogs")]
    public virtual FoodItem Food { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("FoodLogs")]
    public virtual User User { get; set; } = null!;
}
