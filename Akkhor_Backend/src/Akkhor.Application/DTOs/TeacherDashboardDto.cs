namespace Akkhor.Application.DTOs.Teacher;

public class TeacherDashboardDto
{
    public int TotalClasses { get; set; }

    public int TotalAssignments { get; set; }

    public int PublishedAssignments { get; set; }

    public int DraftAssignments { get; set; }

    public int UpcomingAssignments { get; set; }

    public List<TeacherDashboardAssignmentDto> RecentAssignments { get; set; }
        = new();
}

public class TeacherDashboardAssignmentDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Status { get; set; }

    public string? ClassName { get; set; }

    public string? CourseName { get; set; }

    public string? SubjectName { get; set; }
}