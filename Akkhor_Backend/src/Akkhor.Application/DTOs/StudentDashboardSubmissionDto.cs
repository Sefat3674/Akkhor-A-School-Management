namespace Akkhor.Application.DTOs.StudentDashboard;

public class StudentDashboardSubmissionDto
{
    public Guid Id { get; set; }

    public Guid AssignmentId { get; set; }

    public string AssignmentTitle { get; set; }
        = string.Empty;

    public DateTime SubmittedAt { get; set; }

    public decimal? MarksObtained { get; set; }

    public decimal? TotalMarks { get; set; }

    public bool IsGraded { get; set; }

    public string Status { get; set; } = string.Empty;
}