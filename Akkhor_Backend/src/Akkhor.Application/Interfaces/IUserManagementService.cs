using Akkhor.Application.DTOs.UserManagement;


namespace Akkhor.Application.Interfaces;


public interface IUserManagementService
{

    Task<List<UserDto>> GetAllUsersAsync();


    Task<UserDto?> GetUserByIdAsync(string id);


    Task<bool> CreateUserAsync(CreateUserDto dto);


    Task<bool> UpdateUserAsync(
        string id,
        UpdateUserDto dto);


    Task<bool> DeleteUserAsync(string id);


    Task<bool> AssignRoleAsync(
        AssignRoleDto dto);


    Task<bool> ResetPasswordAsync(
        string userId,
        string newPassword);
    Task<bool> UpdateUserStatusAsync(
    string userId,
    bool isActive
);
    Task<List<RoleDto>> GetRolesAsync();
    Task<List<string>> GetUserRolesAsync(string userId);


}