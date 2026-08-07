namespace Akkhor.Application.DTOs.Classes;

public class CreateClassDto
{

    public Guid AcademicYearId { get; set; }


    public string Name { get; set; } = string.Empty;


    public string Code { get; set; } = string.Empty;


    public string? Description { get; set; }


    public int DisplayOrder { get; set; }

}