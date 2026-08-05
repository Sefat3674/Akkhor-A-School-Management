using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Data;

public class ApplicationDbContext
    : IdentityDbContext<Users, Roles, string>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Users>()
            .ToTable("Users");


        builder.Entity<Roles>()
            .ToTable("Roles");


        builder.Entity<IdentityUserRole<string>>()
            .ToTable("UserRoles");


        builder.Entity<IdentityUserClaim<string>>()
            .ToTable("UserClaims");


        builder.Entity<IdentityRoleClaim<string>>()
            .ToTable("RoleClaims");


        builder.Entity<IdentityUserLogin<string>>()
            .ToTable("UserLogins");


        builder.Entity<IdentityUserToken<string>>()
            .ToTable("UserTokens");
    }
}