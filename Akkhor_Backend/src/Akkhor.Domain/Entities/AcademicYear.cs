using System.ComponentModel.DataAnnotations;

namespace Akkhor.Domain.Entities;

public class AcademicYear
{
    public Guid Id { get; set; }


    [Required]
    public string Name { get; set; } = string.Empty;



    public DateOnly StartDate { get; set; }



    public DateOnly EndDate { get; set; }



    public bool IsActive { get; set; } = true;



    public DateTime CreatedAt { get; set; }
     = DateTime.UtcNow;



    public string? CreatedBy { get; set; }



    public DateTime? UpdatedAt { get; set; }



    public string? UpdatedBy { get; set; }



    // Navigation

    public ICollection<Class> Classes { get; set; }
        = new List<Class>();
}