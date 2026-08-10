using Akkhor.Application.DTOs.Assignments;
using Akkhor.Application.DTOs.StudentDashboard;
using Akkhor.Application.Interfaces;
using Akkhor.Application.Interfaces.Services;

namespace Akkhor.Application.Services;

public class StudentDashboardService : IStudentDashboardService
{
    private readonly IAssignmentService _assignmentService;
    private readonly IAssignmentSubmissionService _submissionService;
    private readonly IUserRepository _userRepository;

    public StudentDashboardService(
        IAssignmentService assignmentService,
        IAssignmentSubmissionService submissionService,
        IUserRepository userRepository)
    {
        _assignmentService = assignmentService;
        _submissionService = submissionService;
        _userRepository = userRepository;
    }

    // =====================================================
    // GET COMPLETE STUDENT DASHBOARD
    // =====================================================

    public async Task<StudentDashboardDto?> GetDashboardAsync(
        string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            return null;
        }

        // =================================================
        // GET STUDENT
        // =================================================

        var student =
            await _userRepository.GetByIdAsync(studentId);

        if (student == null)
        {
            return null;
        }

        // =================================================
        // GET ASSIGNMENTS
        // =================================================

        var assignments =
            await _assignmentService
                .GetAssignmentsForStudentAsync(studentId)
            ?? Enumerable.Empty<AssignmentDto>();

        // =================================================
        // GET SUBMISSIONS
        // =================================================

        var submissions =
            await _submissionService
                .GetMySubmissionsAsync(studentId)
            ?? Enumerable.Empty<AssignmentSubmissionDto>();

        var assignmentList = assignments.ToList();
        var submissionList = submissions.ToList();

        var now = DateTime.UtcNow;

        // =================================================
        // COUNTS
        // =================================================

        var totalAssignments =
            assignmentList.Count;

        var submittedAssignments =
            assignmentList.Count(a =>
                submissionList.Any(s =>
                    s.AssignmentId == a.Id));

        var pendingAssignments =
            assignmentList.Count(a =>
                !submissionList.Any(s =>
                    s.AssignmentId == a.Id) &&
                a.Deadline >= now);

        var gradedAssignments =
            submissionList.Count(IsGraded);

        var overdueAssignments =
            assignmentList.Count(a =>
                a.Deadline < now &&
                !submissionList.Any(s =>
                    s.AssignmentId == a.Id));

        // =================================================
        // RECENT ASSIGNMENTS
        // =================================================

        var recentAssignments =
            assignmentList
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a =>
                    MapAssignment(
                        a,
                        submissionList))
                .ToList();

        // =================================================
        // UPCOMING ASSIGNMENTS
        // =================================================

        var upcomingAssignments =
            assignmentList
                .Where(a =>
                    a.Deadline >= now &&
                    !submissionList.Any(s =>
                        s.AssignmentId == a.Id))
                .OrderBy(a => a.Deadline)
                .Take(5)
                .Select(a =>
                    MapAssignment(
                        a,
                        submissionList))
                .ToList();

        // =================================================
        // RECENT SUBMISSIONS
        // =================================================

        var recentSubmissions =
            submissionList
                .OrderByDescending(s => s.SubmittedAt)
                .Take(5)
                .Select(s =>
                    MapSubmission(
                        s,
                        assignmentList))
                .ToList();

        // =================================================
        // RETURN
        // =================================================

        return new StudentDashboardDto
        {
            StudentId = studentId,

            // Student information
            StudentName = GetStudentName(student),
            Email = GetStudentEmail(student),

            // Statistics
            TotalAssignments = totalAssignments,

            PendingAssignments =
                pendingAssignments,

            SubmittedAssignments =
                submittedAssignments,

            GradedAssignments =
                gradedAssignments,

            OverdueAssignments =
                overdueAssignments,

            RecentAssignments =
                recentAssignments,

            UpcomingAssignments =
                upcomingAssignments,

            RecentSubmissions =
                recentSubmissions
        };
    }

    // =====================================================
    // GET STATISTICS
    // =====================================================

    public async Task<StudentDashboardStatisticsDto>
        GetStatisticsAsync(
            string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException(
                "Student ID is required.");
        }

        var assignments =
            await _assignmentService
                .GetAssignmentsForStudentAsync(studentId)
            ?? Enumerable.Empty<AssignmentDto>();

        var submissions =
            await _submissionService
                .GetMySubmissionsAsync(studentId)
            ?? Enumerable.Empty<AssignmentSubmissionDto>();

        var assignmentList =
            assignments.ToList();

        var submissionList =
            submissions.ToList();

        var total =
            assignmentList.Count;

        var submitted =
            assignmentList.Count(a =>
                submissionList.Any(s =>
                    s.AssignmentId == a.Id));

        var pending =
            assignmentList.Count(a =>
                !submissionList.Any(s =>
                    s.AssignmentId == a.Id) &&
                a.Deadline >= DateTime.UtcNow);

        var graded =
            submissionList.Count(IsGraded);

        var overdue =
            assignmentList.Count(a =>
                a.Deadline < DateTime.UtcNow &&
                !submissionList.Any(s =>
                    s.AssignmentId == a.Id));

        // =================================================
        // SUBMISSION RATE
        // =================================================

        decimal submissionRate = 0;

        if (total > 0)
        {
            submissionRate =
                Math.Round(
                    (decimal)submitted /
                    total *
                    100,
                    2);
        }

        // =================================================
        // AVERAGE MARKS
        // =================================================

        var gradedSubmissions =
            submissionList
                .Where(s =>
                    s.MarksObtained.HasValue)
                .ToList();

        decimal averageMarks = 0;

        if (gradedSubmissions.Count > 0)
        {
            averageMarks =
                Math.Round(
                    gradedSubmissions
                        .Average(s =>
                            s.MarksObtained!.Value),
                    2);
        }

        // =================================================
        // AVERAGE PERCENTAGE
        // =================================================

        decimal averagePercentage = 0;

        if (gradedSubmissions.Count > 0)
        {
            decimal totalPercentage = 0;
            int validCount = 0;

            foreach (var submission in gradedSubmissions)
            {
                var assignment =
                    assignmentList.FirstOrDefault(a =>
                        a.Id == submission.AssignmentId);

                if (assignment == null ||
                    assignment.MaximumMarks <= 0)
                {
                    continue;
                }

                totalPercentage +=
                    (submission.MarksObtained!.Value /
                     assignment.MaximumMarks) *
                    100;

                validCount++;
            }

            if (validCount > 0)
            {
                averagePercentage =
                    Math.Round(
                        totalPercentage /
                        validCount,
                        2);
            }
        }

        return new StudentDashboardStatisticsDto
        {
            TotalAssignments =
                total,

            PendingAssignments =
                pending,

            SubmittedAssignments =
                submitted,

            GradedAssignments =
                graded,

            OverdueAssignments =
                overdue,

            SubmissionRate =
                submissionRate,

            AverageMarks =
                averageMarks,

            // Add this property to DTO
            AveragePercentage =
                averagePercentage
        };
    }

    // =====================================================
    // GET RECENT ASSIGNMENTS
    // =====================================================

    public async Task<List<StudentDashboardAssignmentDto>>
        GetRecentAssignmentsAsync(
            string studentId,
            int limit)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException(
                "Student ID is required.");
        }

        if (limit <= 0)
        {
            return [];
        }

        var assignments =
            await _assignmentService
                .GetAssignmentsForStudentAsync(studentId)
            ?? Enumerable.Empty<AssignmentDto>();

        var submissions =
            await _submissionService
                .GetMySubmissionsAsync(studentId)
            ?? Enumerable.Empty<AssignmentSubmissionDto>();

        return assignments
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .Select(a =>
                MapAssignment(
                    a,
                    submissions))
            .ToList();
    }

    // =====================================================
    // GET UPCOMING ASSIGNMENTS
    // =====================================================

    public async Task<List<StudentDashboardAssignmentDto>>
        GetUpcomingAssignmentsAsync(
            string studentId,
            int limit)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException(
                "Student ID is required.");
        }

        if (limit <= 0)
        {
            return [];
        }

        var assignments =
            await _assignmentService
                .GetAssignmentsForStudentAsync(studentId)
            ?? Enumerable.Empty<AssignmentDto>();

        var submissions =
            await _submissionService
                .GetMySubmissionsAsync(studentId)
            ?? Enumerable.Empty<AssignmentSubmissionDto>();

        var now =
            DateTime.UtcNow;

        return assignments
            .Where(a =>
                a.Deadline >= now &&
                !submissions.Any(s =>
                    s.AssignmentId == a.Id))
            .OrderBy(a => a.Deadline)
            .Take(limit)
            .Select(a =>
                MapAssignment(
                    a,
                    submissions))
            .ToList();
    }

    // =====================================================
    // GET RECENT SUBMISSIONS
    // =====================================================

    public async Task<List<StudentDashboardSubmissionDto>>
        GetRecentSubmissionsAsync(
            string studentId,
            int limit)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException(
                "Student ID is required.");
        }

        if (limit <= 0)
        {
            return [];
        }

        var assignments =
            await _assignmentService
                .GetAssignmentsForStudentAsync(studentId)
            ?? Enumerable.Empty<AssignmentDto>();

        var submissions =
            await _submissionService
                .GetMySubmissionsAsync(studentId)
            ?? Enumerable.Empty<AssignmentSubmissionDto>();

        return submissions
            .OrderByDescending(s => s.SubmittedAt)
            .Take(limit)
            .Select(s =>
                MapSubmission(
                    s,
                    assignments))
            .ToList();
    }

    // =====================================================
    // CHECK GRADED
    // =====================================================

    private static bool IsGraded(
        AssignmentSubmissionDto submission)
    {
        return
            submission.MarksObtained.HasValue;
    }

    // =====================================================
    // MAP ASSIGNMENT
    // =====================================================

    private static StudentDashboardAssignmentDto
        MapAssignment(
            AssignmentDto assignment,
            IEnumerable<AssignmentSubmissionDto> submissions)
    {
        var submission =
            submissions.FirstOrDefault(s =>
                s.AssignmentId == assignment.Id);

        var isSubmitted =
            submission != null;

        var isGraded =
            submission != null &&
            IsGraded(submission);

        var isOverdue =
            assignment.Deadline < DateTime.UtcNow &&
            !isSubmitted;

        var status =
            isGraded
                ? "Graded"
                : isSubmitted
                    ? "Submitted"
                    : isOverdue
                        ? "Overdue"
                        : "Pending";

        return new StudentDashboardAssignmentDto
        {
            Id =
                assignment.Id,

            Title =
                assignment.Title,

            Description =
                assignment.Description,

            CourseName =
                assignment.CourseName,

            SubjectName =
                assignment.SubjectName,

            TeacherName =
                assignment.TeacherName,

            DueDate =
                assignment.Deadline,

            TotalMarks =
                Convert.ToInt32(
                    assignment.MaximumMarks),

            IsPublished =
                assignment.IsPublished,

            IsSubmitted =
                isSubmitted,

            IsGraded =
                isGraded,

            IsOverdue =
                isOverdue,

            Status =
                status
        };
    }

    // =====================================================
    // MAP SUBMISSION
    // =====================================================

    private static StudentDashboardSubmissionDto
        MapSubmission(
            AssignmentSubmissionDto submission,
            IEnumerable<AssignmentDto> assignments)
    {
        var assignment =
            assignments.FirstOrDefault(a =>
                a.Id == submission.AssignmentId);

        var isGraded =
            IsGraded(submission);

        return new StudentDashboardSubmissionDto
        {
            Id =
                submission.Id,

            AssignmentId =
                submission.AssignmentId,

            AssignmentTitle =
                submission.AssignmentTitle
                ?? assignment?.Title
                ?? "Assignment",

            SubmittedAt =
                submission.SubmittedAt,

            MarksObtained =
                submission.MarksObtained,

            TotalMarks =
                assignment != null
                    ? Convert.ToInt32(
                        assignment.MaximumMarks)
                    : 0,

            IsGraded =
                isGraded,

            Status =
                isGraded
                    ? "Graded"
                    : "Pending"
        };
    }

    // =====================================================
    // STUDENT NAME
    // =====================================================

    private static string GetStudentName(
        Domain.Entities.Users student)
    {
        if (!string.IsNullOrWhiteSpace(student.FullName))
        {
            return student.FullName;
        }

        if (!string.IsNullOrWhiteSpace(student.UserName))
        {
            return student.UserName;
        }

        return string.Empty;
    }

    // =====================================================
    // STUDENT EMAIL
    // =====================================================

    private static string? GetStudentEmail(
        Domain.Entities.Users student)
    {
        return student.Email;
    }
}