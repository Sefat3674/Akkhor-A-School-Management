namespace Akkhor.Application.DTOs.CourseSubjects;

public class CourseSubjectDto
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string CourseName { get; set; } = string.Empty;

    public Guid SubjectId { get; set; }

    public string SubjectName { get; set; } = string.Empty;

    public bool IsMandatory { get; set; }

    public int DisplayOrder { get; set; }
}