namespace Akkhor.Application.DTOs.AdminDashboard;

public class AdminDashboardDto
{
    // =====================================================
    // SUMMARY
    // =====================================================

    public int TotalUsers { get; set; }

    public int TotalStudents { get; set; }

    public int TotalTeachers { get; set; }

    public int TotalAdmins { get; set; }

    public int TotalClasses { get; set; }

    public int TotalSections { get; set; }

    public int TotalCourses { get; set; }

    public int TotalSubjects { get; set; }

    public int TotalCourseSubjects { get; set; }
    public int TotalAcademicYears { get; set; }

    public int TotalEnrollments { get; set; }

    public int TotalTeacherAssignments { get; set; }

    public int TotalAssignments { get; set; }

    public int TotalSubmissions { get; set; }


    // =====================================================
    // ACADEMIC YEAR
    // =====================================================

    public AcademicYearSummaryDto? ActiveAcademicYear { get; set; }


    // =====================================================
    // ASSIGNMENT
    // =====================================================

    public List<AdminAssignmentSummaryDto>
        RecentAssignments
    { get; set; }
        = new();


    // =====================================================
    // SUBMISSION
    // =====================================================

    public List<AdminSubmissionSummaryDto>
        RecentSubmissions
    { get; set; }
        = new();

    public int PendingSubmissions { get; set; }
}


// =========================================================
// ACADEMIC YEAR SUMMARY
// =========================================================

public class AcademicYearSummaryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }
}


// =========================================================
// ASSIGNMENT SUMMARY
// =========================================================

public class AdminAssignmentSummaryDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsActive { get; set; }

    public int SubmissionCount { get; set; }
}


// =========================================================
// SUBMISSION SUMMARY
// =========================================================

public class AdminSubmissionSummaryDto
{
    public Guid Id { get; set; }

    public Guid AssignmentId { get; set; }

    public string? AssignmentTitle { get; set; }

    public string? StudentId { get; set; }

    public string? StudentName { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public string? Status { get; set; }

    public decimal? Marks { get; set; }
}