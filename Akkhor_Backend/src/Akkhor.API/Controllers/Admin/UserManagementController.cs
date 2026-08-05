using Akkhor.Application.DTOs.UserManagement;
using Akkhor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserManagementController : ControllerBase
{

    private readonly IUserManagementService _userService;


    public UserManagementController(
        IUserManagementService userService)
    {
        _userService = userService;
    }



    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(users);
    }



    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {

        var user =
            await _userService.GetUserByIdAsync(id);


        if (user == null)
            return NotFound(new
            {
                message = "User not found"
            });


        return Ok(user);
    }




    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        CreateUserDto dto)
    {

        var result =
            await _userService.CreateUserAsync(dto);



        if (!result)
        {
            return BadRequest(new
            {
                message = "User creation failed"
            });
        }



        return Ok(new
        {
            message = "User created successfully"
        });

    }





    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        string id,
        UpdateUserDto dto)
    {

        var result =
            await _userService.UpdateUserAsync(
                id,
                dto);



        if (!result)
        {
            return BadRequest(new
            {
                message = "User update failed"
            });
        }



        return Ok(new
        {
            message = "User updated successfully"
        });

    }

    // GET: api/users/roles
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {

        var roles = await _userService.GetRolesAsync();

        return Ok(roles);

    }



    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(
        string id)
    {


        var result =
            await _userService.DeleteUserAsync(id);



        if (!result)
        {
            return BadRequest(new
            {
                message = "User delete failed"
            });
        }



        return Ok(new
        {
            message = "User deactivated successfully"
        });

    }





    // PUT: api/users/assign-role
    [HttpPut("assign-role")]
    public async Task<IActionResult> AssignRole(
        AssignRoleDto dto)
    {


        var result =
            await _userService.AssignRoleAsync(dto);



        if (!result)
        {
            return BadRequest(new
            {
                message = "Role assignment failed"
            });
        }



        return Ok(new
        {
            message = "Role assigned successfully"
        });

    }





    // PUT: api/users/{id}/reset-password
    [HttpPut("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(
        string id,
        [FromBody] string newPassword)
    {


        var result =
            await _userService.ResetPasswordAsync(
                id,
                newPassword);



        if (!result)
        {
            return BadRequest(new
            {
                message = "Password reset failed"
            });
        }



        return Ok(new
        {
            message = "Password reset successfully"
        });

    }
    // PUT: api/users/{id}/status

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateUserStatus(
        string id,
        [FromBody] bool isActive)
    {


        var result =
            await _userService.UpdateUserStatusAsync(
                id,
                isActive
            );


        if (!result)
        {
            return BadRequest(new
            {
                message = "Status update failed"
            });
        }


        return Ok(new
        {
            message =
            isActive
            ?
            "User activated successfully"
            :
            "User deactivated successfully"
        });

    }
    // GET: api/users/{id}/roles

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetUserRoles(
        string id)
    {

        var roles =
            await _userService.GetUserRolesAsync(id);


        return Ok(roles);

    }
}