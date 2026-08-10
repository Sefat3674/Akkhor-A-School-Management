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

    public int TotalAcademicYears { get; set; }

    public int TotalClasses { get; set; }

    public int TotalSections { get; set; }

    public int TotalCourses { get; set; }

    public int TotalSubjects { get; set; }

    public int TotalCourseSubjects { get; set; }

    public int TotalEnrollments { get; set; }

    public int TotalTeacherAssignments { get; set; }

    public int TotalAssignments { get; set; }

    public int TotalSubmissions { get; set; }

    public int PendingSubmissions { get; set; }


    // =====================================================
    // ACTIVE ACADEMIC YEAR
    // =====================================================

    public AcademicYearSummaryDto? ActiveAcademicYear { get; set; }


    // =====================================================
    // RECENT ASSIGNMENTS
    // =====================================================

    public List<RecentAssignmentDto> RecentAssignments { get; set; }
        = new();


    // =====================================================
    // RECENT SUBMISSIONS
    // =====================================================

    public List<RecentSubmissionDto> RecentSubmissions { get; set; }
        = new();
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
// RECENT ASSIGNMENT
// =========================================================

public class RecentAssignmentDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? SubjectName { get; set; }

    public string? TeacherName { get; set; }

    public string? CourseName { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Status { get; set; }
}


// =========================================================
// RECENT SUBMISSION
// =========================================================

public class RecentSubmissionDto
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