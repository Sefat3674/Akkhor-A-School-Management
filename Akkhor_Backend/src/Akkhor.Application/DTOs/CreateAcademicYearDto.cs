using System.ComponentModel.DataAnnotations;

namespace Akkhor.Application.DTOs.AcademicYear;

public class CreateAcademicYearDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;


    [Required]
    public DateOnly StartDate { get; set; }


    [Required]
    public DateOnly EndDate { get; set; }


    public bool IsActive { get; set; } = true;
}