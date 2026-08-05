namespace Akkhor.Domain.Entities;

public class StudentEnrollment
{
    public Guid Id { get; set; }


    // ASP.NET Identity User Id
    public string StudentId { get; set; } = string.Empty;



    public Guid ClassId { get; set; }


    public Guid CourseId { get; set; }


    public Guid? SectionId { get; set; }


    public string? RollNumber { get; set; }


    public DateOnly EnrollmentDate { get; set; }
        = DateOnly.FromDateTime(DateTime.UtcNow);



    public string Status { get; set; } = "Active";


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    // Navigation

    public Users Student { get; set; } = null!;



    public Class Class { get; set; } = null!;


    public Course Course { get; set; } = null!;


    public ClassSection? Section { get; set; }
}