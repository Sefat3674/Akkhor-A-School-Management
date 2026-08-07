namespace Akkhor.Application.DTOs.Classes;

public class ClassDto
{
    public Guid Id { get; set; }


    public Guid AcademicYearId { get; set; }

    public string AcademicYearName { get; set; } = string.Empty;


    public string Name { get; set; } = string.Empty;


    public string Code { get; set; } = string.Empty;


    public string? Description { get; set; }


    public int DisplayOrder { get; set; }


    public bool IsActive { get; set; }


    public int SectionCount { get; set; }


    public DateTime CreatedAt { get; set; }
}