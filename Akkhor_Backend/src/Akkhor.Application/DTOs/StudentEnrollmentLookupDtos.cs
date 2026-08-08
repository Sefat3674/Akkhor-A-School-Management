namespace Akkhor.Application.DTOs.StudentEnrollments;

public class StudentLookupDto
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
}


public class ClassLookupDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}


public class CourseLookupDto
{
    public Guid Id { get; set; }

    public Guid ClassId { get; set; }

    public string CourseName { get; set; } = string.Empty;
}


public class SectionLookupDto
{
    public Guid Id { get; set; }

    public Guid ClassId { get; set; }

    public string SectionName { get; set; } = string.Empty;
}