using Akkhor.Application.DTOs;

namespace Akkhor.Application.Interfaces;

public interface IAuthService
{
    Task<object> RegisterAsync(
        RegisterDto dto
    );


    Task<object> LoginAsync(
        LoginDto dto
    );


    Task<object> GetCurrentUserAsync(
        string userId
    );
}