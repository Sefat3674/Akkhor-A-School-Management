namespace Akkhor.Domain.Entities;

public class Subject
{
    public Guid Id { get; set; }


    public string Name { get; set; } = string.Empty;


    public string Code { get; set; } = string.Empty;


    public string? Description { get; set; }


    public int? CreditHours { get; set; }


    public bool IsActive { get; set; } = true;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    // Navigation

    public ICollection<CourseSubject> CourseSubjects { get; set; }
        = new List<CourseSubject>();
}