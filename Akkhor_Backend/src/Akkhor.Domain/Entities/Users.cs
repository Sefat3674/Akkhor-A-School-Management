using Microsoft.AspNetCore.Identity;

namespace Akkhor.Domain.Entities;

public class Users : IdentityUser
{
    public string? FullName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    
}