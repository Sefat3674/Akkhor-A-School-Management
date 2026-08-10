using Akkhor.Application.DTOs.StudentDashboard;

namespace Akkhor.Application.Interfaces.Services;

public interface IStudentDashboardService
{
    Task<StudentDashboardDto?>
        GetDashboardAsync(
            string studentId);


    Task<StudentDashboardStatisticsDto>
        GetStatisticsAsync(
            string studentId);


    Task<List<StudentDashboardAssignmentDto>>
        GetRecentAssignmentsAsync(
            string studentId,
            int limit);


    Task<List<StudentDashboardAssignmentDto>>
        GetUpcomingAssignmentsAsync(
            string studentId,
            int limit);


    Task<List<StudentDashboardSubmissionDto>>
        GetRecentSubmissionsAsync(
            string studentId,
            int limit);
}