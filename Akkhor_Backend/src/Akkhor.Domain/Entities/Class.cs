namespace Akkhor.Domain.Entities;

public class Class
{
    public Guid Id { get; set; }


    public Guid AcademicYearId { get; set; }


    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }


    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }



    // Navigation

    public AcademicYear AcademicYear { get; set; } = null!;


    public ICollection<ClassSection> Sections { get; set; }
        = new List<ClassSection>();


    public ICollection<Course> Courses { get; set; }
        = new List<Course>();


    public ICollection<StudentEnrollment> StudentEnrollments { get; set; }
        = new List<StudentEnrollment>();
}