using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces;

public interface ITokenService
{
    (string token, DateTime expiresAt) CreateToken(
        Users user,
        IList<string> roles
    );
}