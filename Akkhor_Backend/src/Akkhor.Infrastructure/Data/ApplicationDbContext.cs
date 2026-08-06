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


    // Identity Tables
    public DbSet<Users> Users { get; set; }

    public DbSet<Roles> Roles { get; set; }


    // Academic Module

    public DbSet<AcademicYear> AcademicYears { get; set; }

    public DbSet<Class> Classes { get; set; }

    public DbSet<ClassSection> ClassSections { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<CourseSubject> CourseSubjects { get; set; }

    public DbSet<StudentEnrollment> StudentEnrollments { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        // =====================================================
        // Identity Table Mapping
        // =====================================================

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



        // =====================================================
        // AcademicYears
        // =====================================================

        builder.Entity<AcademicYear>()
            .ToTable("AcademicYears");


        builder.Entity<AcademicYear>()
            .HasMany(x => x.Classes)
            .WithOne(x => x.AcademicYear)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);



        // =====================================================
        // Classes
        // =====================================================

        builder.Entity<Class>()
            .ToTable("Classes");


        builder.Entity<Class>()
            .HasMany(x => x.Sections)
            .WithOne(x => x.Class)
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<Class>()
            .HasMany(x => x.Courses)
            .WithOne(x => x.Class)
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Cascade);



        // =====================================================
        // Class Sections
        // =====================================================

        builder.Entity<ClassSection>()
            .ToTable("ClassSections");



        // =====================================================
        // Courses
        // =====================================================

        builder.Entity<Course>()
            .ToTable("Courses");


        builder.Entity<Course>()
            .HasMany(x => x.CourseSubjects)
            .WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);



        // =====================================================
        // Subjects
        // =====================================================

        builder.Entity<Subject>()
            .ToTable("Subjects");


        builder.Entity<Subject>()
            .HasMany(x => x.CourseSubjects)
            .WithOne(x => x.Subject)
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);



        // =====================================================
        // Course Subjects
        // =====================================================

        builder.Entity<CourseSubject>()
            .ToTable("CourseSubjects");


        builder.Entity<CourseSubject>()
            .HasIndex(x => new
            {
                x.CourseId,
                x.SubjectId
            })
            .IsUnique();



        // =====================================================
        // Student Enrollment
        // =====================================================

        builder.Entity<StudentEnrollment>()
            .ToTable("StudentEnrollments");


        builder.Entity<StudentEnrollment>()
            .HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<StudentEnrollment>()
            .HasOne(x => x.Class)
            .WithMany(x => x.StudentEnrollments)
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<StudentEnrollment>()
            .HasOne(x => x.Course)
            .WithMany(x => x.StudentEnrollments)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<StudentEnrollment>()
            .HasOne(x => x.Section)
            .WithMany(x => x.StudentEnrollments)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.SetNull);



        // =====================================================
        // PostgreSQL Column Mapping
        // =====================================================

        builder.Entity<StudentEnrollment>()
            .Property(x => x.StudentId)
            .HasMaxLength(450);


        builder.Entity<AcademicYear>()
            .Property(x => x.CreatedBy)
            .HasMaxLength(450);


        builder.Entity<Class>()
            .Property(x => x.CreatedBy)
            .HasMaxLength(450);


        builder.Entity<Class>()
            .Property(x => x.UpdatedBy)
            .HasMaxLength(450);


        // =====================================================
        // Academic Year Date Configuration
        // =====================================================

        builder.Entity<AcademicYear>()
            .Property(x => x.StartDate)
            .HasColumnType("date");


        builder.Entity<AcademicYear>()
            .Property(x => x.EndDate)
            .HasColumnType("date");



        builder.Entity<AcademicYear>()
     .Property(x => x.CreatedAt)
     .HasColumnType("timestamp with time zone");


        builder.Entity<AcademicYear>()
            .Property(x => x.UpdatedAt)
            .HasColumnType("timestamp with time zone");
    }
}