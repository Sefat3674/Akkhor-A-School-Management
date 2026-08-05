namespace Akkhor.Domain.Entities;

public class ClassSection
{
    public Guid Id { get; set; }


    public Guid ClassId { get; set; }


    public string SectionName { get; set; } = string.Empty;


    public string? RoomNumber { get; set; }


    public int? Capacity { get; set; }


    public bool IsActive { get; set; } = true;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    // Navigation

    public Class Class { get; set; } = null!;


    public ICollection<StudentEnrollment> StudentEnrollments { get; set; }
        = new List<StudentEnrollment>();
}