namespace Akkhor.Application.DTOs.Sections;

public class CreateSectionDto
{
    public Guid ClassId { get; set; }

    public string SectionName { get; set; } = string.Empty;

    public string? RoomNumber { get; set; }

    public int? Capacity { get; set; }
}