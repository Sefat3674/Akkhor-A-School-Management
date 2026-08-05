namespace Akkhor.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string UserId { get; set; } = null!;


    public AuthResponseDto(
        string token,
        DateTime expiresAt,
        string? fullName,
        string? email,
        string userId)
    {
        Token = token;
        ExpiresAt = expiresAt;
        FullName = fullName;
        Email = email;
        UserId = userId;
    }
}