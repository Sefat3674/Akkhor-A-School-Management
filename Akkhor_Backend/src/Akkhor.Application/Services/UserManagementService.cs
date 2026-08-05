using Akkhor.Application.DTOs.UserManagement;
using Akkhor.Application.Interfaces;
using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Application.Services;

public class UserManagementService : IUserManagementService
{

    private readonly UserManager<Users> _userManager;
    private readonly RoleManager<Roles> _roleManager;


    public UserManagementService(
        UserManager<Users> userManager,
        RoleManager<Roles> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }



    // GET ALL USERS
    public async Task<List<UserDto>> GetAllUsersAsync()
    {

        var users = await _userManager.Users.ToListAsync();

        var result = new List<UserDto>();


        foreach (var user in users)
        {

            var roles = await _userManager.GetRolesAsync(user);


            result.Add(new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            });

        }


        return result;

    }



    // GET USER BY ID
    public async Task<UserDto?> GetUserByIdAsync(string id)
    {

        var user = await _userManager.FindByIdAsync(id);


        if (user == null)
            return null;


        var roles = await _userManager.GetRolesAsync(user);


        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            Roles = roles.ToList()
        };

    }



    // CREATE USER
    public async Task<bool> CreateUserAsync(CreateUserDto dto)
    {

        var user = new Users
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            IsActive = true
        };


        var result = await _userManager
            .CreateAsync(user, dto.Password);



        if (!result.Succeeded)
            return false;



        if (!string.IsNullOrEmpty(dto.Role))
        {
            await _userManager.AddToRoleAsync(
                user,
                dto.Role
            );
        }


        return true;

    }




    // UPDATE USER
    public async Task<bool> UpdateUserAsync(
    string id,
    UpdateUserDto dto)
    {

        var user =
            await _userManager.FindByIdAsync(id);


        if (user == null)
            return false;



        // Update basic information
        user.FullName = dto.FullName;
        user.PhoneNumber = dto.PhoneNumber;
        user.IsActive = dto.IsActive;



        var updateResult =
            await _userManager.UpdateAsync(user);



        if (!updateResult.Succeeded)
            return false;





        // ============================
        // UPDATE ROLE
        // ============================

        if (!string.IsNullOrEmpty(dto.Role))
        {


            // Existing roles
            var currentRoles =
                await _userManager.GetRolesAsync(user);



            // Remove old roles

            if (currentRoles.Any())
            {

                var removeResult =
                    await _userManager.RemoveFromRolesAsync(
                        user,
                        currentRoles
                    );


                if (!removeResult.Succeeded)
                    return false;

            }





            // Check role exists

            var roleExists =
                await _roleManager.RoleExistsAsync(dto.Role);



            if (!roleExists)
                return false;





            // Add new role

            var addRoleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    dto.Role
                );



            if (!addRoleResult.Succeeded)
                return false;

        }




        return true;

    }




    // DELETE USER
    // DELETE USER PERMANENTLY
    public async Task<bool> DeleteUserAsync(string id)
    {

        var user =
            await _userManager.FindByIdAsync(id);


        if (user == null)
            return false;



        // Remove user from all roles first
        var roles =
            await _userManager.GetRolesAsync(user);



        if (roles.Any())
        {

            var removeRolesResult =
                await _userManager.RemoveFromRolesAsync(
                    user,
                    roles
                );


            if (!removeRolesResult.Succeeded)
                return false;

        }



        // Delete user permanently
        var result =
            await _userManager.DeleteAsync(user);



        return result.Succeeded;

    }
    public async Task<List<RoleDto>> GetRolesAsync()
    {

        var roles = await _roleManager.Roles
            .ToListAsync();


        return roles.Select(x => new RoleDto
        {
            Id = x.Id,
            Name = x.Name

        }).ToList();

    }
    public async Task<List<string>> GetUserRolesAsync(
    string userId)
    {

        var user =
            await _userManager.FindByIdAsync(userId);



        if (user == null)
        {
            return new List<string>();
        }



        var roles =
            await _userManager.GetRolesAsync(user);



        return roles.ToList();

    }




    // ASSIGN ROLE
    public async Task<bool> AssignRoleAsync(
        AssignRoleDto dto)
    {

        var user =
            await _userManager.FindByIdAsync(dto.UserId);



        if (user == null)
            return false;



        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            return false;
        }



        var result =
            await _userManager.AddToRoleAsync(
                user,
                dto.Role
            );



        return result.Succeeded;

    }





    // RESET PASSWORD
    public async Task<bool> ResetPasswordAsync(
        string userId,
        string newPassword)
    {


        var user =
            await _userManager.FindByIdAsync(userId);



        if (user == null)
            return false;



        var token =
            await _userManager.GeneratePasswordResetTokenAsync(user);



        var result =
            await _userManager.ResetPasswordAsync(
                user,
                token,
                newPassword
            );



        return result.Succeeded;

    }
    public async Task<bool> UpdateUserStatusAsync(
       string userId,
       bool isActive)
    {


        var user =
            await _userManager.FindByIdAsync(userId);



        if (user == null)
            return false;




        user.IsActive = isActive;



        var result =
            await _userManager.UpdateAsync(user);



        return result.Succeeded;


    }




}