using System;

namespace Akkhor.Domain.Entities;

public class ApplicationSetting
{
    // =====================================================
    // PRIMARY KEY
    // =====================================================

    public Guid Id { get; set; }


    // =====================================================
    // SETTING INFORMATION
    // =====================================================

    /// <summary>
    /// Unique setting key.
    /// Example: SchoolName
    /// </summary>
    public string Key { get; set; } = string.Empty;


    /// <summary>
    /// Setting value stored as text.
    /// Example: Akkhor School
    /// </summary>
    public string? Value { get; set; }


    /// <summary>
    /// Setting category.
    /// Example: General, Assignment, Security
    /// </summary>
    public string Category { get; set; } = "General";


    /// <summary>
    /// Data type of the value.
    /// Example: string, boolean, integer
    /// </summary>
    public string DataType { get; set; } = "string";


    /// <summary>
    /// Description shown to administrators.
    /// </summary>
    public string? Description { get; set; }


    // =====================================================
    // STATUS
    // =====================================================

    public bool IsActive { get; set; } = true;


    // =====================================================
    // AUDIT
    // =====================================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}