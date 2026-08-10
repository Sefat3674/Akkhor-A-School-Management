namespace Akkhor.Application.DTOs.StudentDashboard;

public class StudentDashboardStatisticsDto
{
    public int TotalAssignments { get; set; }

    public int PendingAssignments { get; set; }

    public int SubmittedAssignments { get; set; }

    public int GradedAssignments { get; set; }

    public int OverdueAssignments { get; set; }

    public decimal SubmissionRate { get; set; }

    public decimal AverageMarks { get; set; }

    public decimal AveragePercentage { get; set; }

    // Angular compatibility
    public int GradedSubmissions
    {
        get => GradedAssignments;
        set => GradedAssignments = value;
    }
}