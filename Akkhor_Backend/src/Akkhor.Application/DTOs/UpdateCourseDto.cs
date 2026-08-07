namespace Akkhor.Application.DTOs.Courses;

public class UpdateCourseDto
{
    public string CourseName { get; set; } = string.Empty;


    public string CourseCode { get; set; } = string.Empty;


    public string? Description { get; set; }


    public int? DurationMonths { get; set; }


    public bool IsActive { get; set; }
}