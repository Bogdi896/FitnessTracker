using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("Goal")]
public partial class Goal
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string GoalType { get; set; } = null!;

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TargetValue { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsAchieved { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Goals")]
    public virtual User User { get; set; } = null!;
}
