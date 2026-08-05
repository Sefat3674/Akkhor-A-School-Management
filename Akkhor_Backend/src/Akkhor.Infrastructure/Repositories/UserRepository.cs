using Akkhor.Application.Interfaces;
using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Akkhor.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<Users> _userManager;


    public UserRepository(
        UserManager<Users> userManager)
    {
        _userManager = userManager;
    }



    public async Task<Users?> GetByEmailAsync(
        string email)
    {
        return await _userManager
            .FindByEmailAsync(email);
    }



    public async Task<Users?> GetByIdAsync(
        string id)
    {
        return await _userManager
            .FindByIdAsync(id);
    }



    public async Task<bool> EmailExistsAsync(
        string email)
    {
        var user =
            await _userManager
            .FindByEmailAsync(email);

        return user != null;
    }



    public async Task<Users> CreateAsync(
        Users user,
        string password)
    {
        var result =
            await _userManager
            .CreateAsync(
                user,
                password
            );


        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(
                    ",",
                    result.Errors
                    .Select(x => x.Description)
                )
            );
        }


        return user;
    }



    public async Task<bool> CheckPasswordAsync(
        Users user,
        string password)
    {
        return await _userManager
            .CheckPasswordAsync(
                user,
                password
            );
    }
    public async Task AddRoleAsync(
    Users user,
    string role)
    {
        var result =
            await _userManager
            .AddToRoleAsync(user, role);


        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(",",
                result.Errors.Select(x => x.Description))
            );
        }
    }



    public async Task<IList<string>> GetRolesAsync(
        Users user)
    {
        return await _userManager
            .GetRolesAsync(user);
    }
}