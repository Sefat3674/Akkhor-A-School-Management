
namespace Akkhor.Application.DTOs.ApplicationSettings;

public class UpdateApplicationSettingDto
{
    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    public string Category { get; set; } = "General";

    public string DataType { get; set; } = "string";

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}

