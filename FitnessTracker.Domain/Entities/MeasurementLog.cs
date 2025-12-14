using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Domain.Entities;

[Table("MeasurementLog")]
public partial class MeasurementLog
{
    [Key]
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Weight { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? BodyFatPercentage { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? WaistCircumference { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Chest { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Arms { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("MeasurementLogs")]
    public virtual User User { get; set; } = null!;
}
