using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces;

public interface IUserRepository
{
    Task<Users?> GetByEmailAsync(string email);

    Task<Users?> GetByIdAsync(string id);

    Task<bool> EmailExistsAsync(string email);

    Task<Users> CreateAsync(
        Users user,
        string password
    );

    Task<bool> CheckPasswordAsync(
        Users user,
        string password
    );
    Task AddRoleAsync(
    Users user,
    string role
);


    Task<IList<string>> GetRolesAsync(
        Users user
    );
}