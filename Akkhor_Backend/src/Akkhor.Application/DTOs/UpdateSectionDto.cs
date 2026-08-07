namespace Akkhor.Application.DTOs.Sections;

public class UpdateSectionDto
{
    public string SectionName { get; set; } = string.Empty;

    public string? RoomNumber { get; set; }

    public int? Capacity { get; set; }

    public bool IsActive { get; set; }
}