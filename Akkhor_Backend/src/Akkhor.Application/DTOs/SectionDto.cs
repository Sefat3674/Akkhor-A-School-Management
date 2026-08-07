namespace Akkhor.Application.DTOs.Sections;

public class SectionDto
{
    public Guid Id { get; set; }

    public Guid ClassId { get; set; }

    public string ClassName { get; set; } = string.Empty;

    public string SectionName { get; set; } = string.Empty;

    public string? RoomNumber { get; set; }

    public int? Capacity { get; set; }

    public bool IsActive { get; set; }

    public int StudentCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}