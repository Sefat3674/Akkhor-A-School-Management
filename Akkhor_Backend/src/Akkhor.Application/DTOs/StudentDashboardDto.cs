namespace Akkhor.Application.DTOs.StudentDashboard;

public class StudentDashboardDto
{
    // =====================================================
    // STUDENT
    // =====================================================

    public string StudentId { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? ClassName { get; set; }

    public string? SectionName { get; set; }

    public string? AcademicYearName { get; set; }


    // =====================================================
    // STATISTICS
    // =====================================================

    public int TotalAssignments { get; set; }

    public int PendingAssignments { get; set; }

    public int SubmittedAssignments { get; set; }

    public int GradedAssignments { get; set; }

    public int OverdueAssignments { get; set; }

    public decimal SubmissionRate { get; set; }

    public decimal AverageMarks { get; set; }


    // =====================================================
    // LISTS
    // =====================================================

    public List<StudentDashboardAssignmentDto>
        RecentAssignments
    { get; set; } = new();

    public List<StudentDashboardAssignmentDto>
        UpcomingAssignments
    { get; set; } = new();

    public List<StudentDashboardSubmissionDto>
        RecentSubmissions
    { get; set; } = new();
}