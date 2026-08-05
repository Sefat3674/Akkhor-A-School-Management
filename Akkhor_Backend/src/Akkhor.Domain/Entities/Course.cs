namespace Akkhor.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }


    public Guid ClassId { get; set; }


    public string CourseName { get; set; } = string.Empty;


    public string CourseCode { get; set; } = string.Empty;


    public string? Description { get; set; }


    public int? DurationMonths { get; set; }


    public bool IsActive { get; set; } = true;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    // Navigation

    public Class Class { get; set; } = null!;


    public ICollection<CourseSubject> CourseSubjects { get; set; }
        = new List<CourseSubject>();


    public ICollection<StudentEnrollment> StudentEnrollments { get; set; }
        = new List<StudentEnrollment>();
}