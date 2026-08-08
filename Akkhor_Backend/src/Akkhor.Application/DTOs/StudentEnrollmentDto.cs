namespace Akkhor.Application.DTOs.StudentEnrollments;

public class StudentEnrollmentDto
{
    public Guid Id { get; set; }


    public string StudentId { get; set; } = "";


    public string StudentName { get; set; } = "";


    public Guid ClassId { get; set; }


    public string ClassName { get; set; } = "";


    public Guid CourseId { get; set; }


    public string CourseName { get; set; } = "";


    public Guid? SectionId { get; set; }


    public string? SectionName { get; set; }


    public string? RollNumber { get; set; }


    public DateOnly EnrollmentDate { get; set; }


    public string Status { get; set; } = "";


    public DateTime CreatedAt { get; set; }


    public DateTime? UpdatedAt { get; set; }
}