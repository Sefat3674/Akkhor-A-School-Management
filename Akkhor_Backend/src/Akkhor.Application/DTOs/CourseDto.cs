namespace Akkhor.Application.DTOs.Courses;

public class CourseDto
{
    public Guid Id { get; set; }

    public Guid ClassId { get; set; }

    public string ClassName { get; set; } = string.Empty;

    public string CourseName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int? DurationMonths { get; set; }

    public bool IsActive { get; set; }

    public int SubjectCount { get; set; }

    public int StudentCount { get; set; }

    public DateTime CreatedAt { get; set; }
}