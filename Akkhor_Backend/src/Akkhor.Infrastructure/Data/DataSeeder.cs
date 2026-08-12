using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Akkhor.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        UserManager<Users> userManager,
        RoleManager<Roles> roleManager)
    {
        // =====================================================
        // Roles
        // =====================================================

        string[] roles =
        {
            "Admin",
            "Teacher",
            "Student"
        };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Roles
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant(),
                    Description = $"{roleName} role",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var roleResult = await roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role {roleName}: " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        // =====================================================
        // Admin
        // =====================================================

        await CreateUserAsync(
            userManager,
            email: "admin@akkhor.com",
            password: "Admin@12345",
            fullName: "Akkhor Administrator",
            role: "Admin");

        // =====================================================
        // Teacher
        // =====================================================

        await CreateUserAsync(
            userManager,
            email: "teacher@akkhor.com",
            password: "Teacher@12345",
            fullName: "Demo Teacher",
            role: "Teacher");

        // =====================================================
        // Student
        // =====================================================

        await CreateUserAsync(
            userManager,
            email: "student@akkhor.com",
            password: "Student@12345",
            fullName: "Demo Student",
            role: "Student");
    }


    // =====================================================
    // Create User
    // =====================================================

    private static async Task CreateUserAsync(
        UserManager<Users> userManager,
        string email,
        string password,
        string fullName,
        string role)
    {
        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            // Make sure the demo user remains active
            if (!existingUser.IsActive)
            {
                existingUser.IsActive = true;
                await userManager.UpdateAsync(existingUser);
            }

            // Make sure the correct role exists
            if (!await userManager.IsInRoleAsync(existingUser, role))
            {
                var roleResult =
                    await userManager.AddToRoleAsync(existingUser, role);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        $"Failed to assign role {role} to {email}: " +
                        string.Join(
                            ", ",
                            roleResult.Errors.Select(e => e.Description)));
                }
            }

            return;
        }

        var user = new Users
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createResult =
            await userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            throw new Exception(
                $"Failed to create user {email}: " +
                string.Join(
                    ", ",
                    createResult.Errors.Select(e => e.Description)));
        }

        var addRoleResult =
            await userManager.AddToRoleAsync(user, role);

        if (!addRoleResult.Succeeded)
        {
            throw new Exception(
                $"Failed to assign role {role} to {email}: " +
                string.Join(
                    ", ",
                    addRoleResult.Errors.Select(e => e.Description)));
        }
    }
}