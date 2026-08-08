namespace Akkhor.Application.DTOs.TeacherClasses;

public class TeacherClassDto
{
    public Guid AssignmentId { get; set; }

    public string TeacherId { get; set; } = string.Empty;

    // Academic Year
    public Guid AcademicYearId { get; set; }
    public string AcademicYearName { get; set; } = string.Empty;

    // Class
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;

    // Section
    public Guid? SectionId { get; set; }
    public string? SectionName { get; set; }

    // Room
    public string? RoomNumber { get; set; }

    // Course
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;

    // Subject
    public Guid SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;

    // Assignment
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
}