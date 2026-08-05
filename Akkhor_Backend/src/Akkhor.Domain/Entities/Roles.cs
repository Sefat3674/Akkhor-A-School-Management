using Microsoft.AspNetCore.Identity;

namespace Akkhor.Domain.Entities;

public class Roles : IdentityRole
{
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;


   
}