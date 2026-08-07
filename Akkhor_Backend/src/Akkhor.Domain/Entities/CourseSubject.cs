namespace Akkhor.Domain.Entities;

public class CourseSubject
{
    public Guid Id { get; set; }


    public Guid CourseId { get; set; }


    public Guid SubjectId { get; set; }


    public bool IsMandatory { get; set; } = true;


    public int DisplayOrder { get; set; } = 0;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }



    // Navigation

    public Course Course { get; set; } = null!;


    public Subject Subject { get; set; } = null!;
}