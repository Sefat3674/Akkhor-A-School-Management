namespace Akkhor.Application.DTOs.StudentDashboard;

public class StudentDashboardAssignmentDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? CourseName { get; set; }

    public string? SubjectName { get; set; }

    public string? TeacherName { get; set; }

    public DateTime? DueDate { get; set; }

    public int TotalMarks { get; set; }

    public bool IsPublished { get; set; }

    public bool IsSubmitted { get; set; }

    public bool IsGraded { get; set; }

    public bool IsOverdue { get; set; }

    public string Status { get; set; } = string.Empty;
}