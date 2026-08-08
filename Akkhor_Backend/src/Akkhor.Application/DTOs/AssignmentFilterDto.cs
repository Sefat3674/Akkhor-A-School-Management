using System;

namespace Akkhor.Application.DTOs.Assignments;

public class AssignmentFilterDto
{
    public Guid? AcademicYearId { get; set; }

    public Guid? ClassId { get; set; }

    public Guid? SectionId { get; set; }

    public Guid? CourseId { get; set; }

    public Guid? SubjectId { get; set; }

    public bool? IsPublished { get; set; }

    public bool? IsActive { get; set; }

    public string? SearchTerm { get; set; }
}