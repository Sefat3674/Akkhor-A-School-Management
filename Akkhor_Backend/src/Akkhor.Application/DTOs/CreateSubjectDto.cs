using System.ComponentModel.DataAnnotations;

namespace Akkhor.Application.DTOs.Subjects;

public class CreateSubjectDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, 50)]
    public int? CreditHours { get; set; }
}