using System.ComponentModel.DataAnnotations;

namespace Akkhor.Application.DTOs.CourseSubjects;

public class UpdateCourseSubjectDto
{
    [Required]
    public Guid CourseId { get; set; }

    [Required]
    public Guid SubjectId { get; set; }

    public bool IsMandatory { get; set; }

    public int DisplayOrder { get; set; }
}