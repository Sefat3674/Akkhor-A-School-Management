using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Akkhor.Infrastructure.Data;

public static class DataSeeder
{

    public static readonly string[] Roles =
    {
        "SuperAdmin",
        "Admin",
        "Principal",
        "VicePrincipal",
        "Teacher",
        "Accountant",
        "Librarian",
        "Student",
        "Parent"
    };


    public static async Task SeedAsync(
        RoleManager<Roles> roleManager,
        UserManager<Users> userManager)
    {

        // ===============================
        // Seed Roles
        // ===============================

        foreach (var role in Roles)
        {

            if (!await roleManager.RoleExistsAsync(role))
            {

                await roleManager.CreateAsync(
                    new Roles
                    {
                        Name = role,

                        NormalizedName =
                            role.ToUpper(),

                        IsActive = true,

                        Description =
                            $"{role} role"
                    });
            }
        }



        // ===============================
        // Seed Super Admin User
        // ===============================


        const string adminEmail =
            "admin@akkhor.edu";


        var admin =
            await userManager
            .FindByEmailAsync(adminEmail);



        if (admin == null)
        {

            admin = new Users
            {
                UserName = adminEmail,

                Email = adminEmail,

                FullName =
                    "System Administrator",


                EmailConfirmed = true,


                IsActive = true,


                CreatedAt =
                    DateTime.UtcNow
            };



            var result =
                await userManager
                .CreateAsync(
                    admin,
                    "Admin@12345"
                );



            if (result.Succeeded)
            {

                await userManager
                    .AddToRoleAsync(
                        admin,
                        "SuperAdmin"
                    );

            }
        }

    }
}