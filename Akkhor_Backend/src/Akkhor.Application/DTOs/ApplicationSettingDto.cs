namespace Akkhor.Application.DTOs.ApplicationSettings;

public class ApplicationSettingDto
{
    public Guid Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    public string Category { get; set; } = string.Empty;

    public string DataType { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}