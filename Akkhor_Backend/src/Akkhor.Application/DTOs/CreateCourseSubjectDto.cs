using System.ComponentModel.DataAnnotations;

namespace Akkhor.Application.DTOs.CourseSubjects;

public class CreateCourseSubjectDto
{
    [Required]
    public Guid CourseId { get; set; }

    [Required]
    public Guid SubjectId { get; set; }

    public bool IsMandatory { get; set; } = true;

    public int DisplayOrder { get; set; } = 0;
}