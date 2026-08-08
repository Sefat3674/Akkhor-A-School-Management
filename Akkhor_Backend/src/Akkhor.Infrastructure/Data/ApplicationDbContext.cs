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

    public DbSet<TeacherAssignment> TeacherAssignments { get; set; }
     public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

    public DbSet<Assignment> Assignments { get; set; }




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


        // =====================================================
        // Course Date Configuration
        // =====================================================

        builder.Entity<Course>()
            .Property(x => x.CreatedAt)
            .HasColumnType("timestamp with time zone");


        builder.Entity<Course>()
            .Property(x => x.UpdatedAt)
            .HasColumnType("timestamp with time zone");


        // =====================================================
        // Teacher Assignment
        // =====================================================

        builder.Entity<TeacherAssignment>()
            .ToTable("TeacherAssignments");


        builder.Entity<TeacherAssignment>()
            .HasKey(x => x.Id);


        // Teacher
        builder.Entity<TeacherAssignment>()
            .HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);


        // Academic Year
        builder.Entity<TeacherAssignment>()
            .HasOne(x => x.AcademicYear)
            .WithMany()
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);


        // Class
        builder.Entity<TeacherAssignment>()
            .HasOne(x => x.Class)
            .WithMany()
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Restrict);


        // Section
        builder.Entity<TeacherAssignment>()
            .HasOne(x => x.Section)
            .WithMany()
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.SetNull);


        // Course
        builder.Entity<TeacherAssignment>()
            .HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);


        // Subject
        builder.Entity<TeacherAssignment>()
            .HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);


        // TeacherId PostgreSQL varchar
        builder.Entity<TeacherAssignment>()
            .Property(x => x.TeacherId)
            .HasMaxLength(450);


        // Audit fields
        builder.Entity<TeacherAssignment>()
            .Property(x => x.CreatedBy)
            .HasMaxLength(450);

        builder.Entity<TeacherAssignment>()
            .Property(x => x.UpdatedBy)
            .HasMaxLength(450);


        // Date fields
        builder.Entity<TeacherAssignment>()
            .Property(x => x.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Entity<TeacherAssignment>()
            .Property(x => x.UpdatedAt)
            .HasColumnType("timestamp with time zone");


        // Prevent duplicate teacher assignment
        builder.Entity<TeacherAssignment>()
            .HasIndex(x => new
            {
                x.TeacherId,
                x.AcademicYearId,
                x.ClassId,
                x.SectionId,
                x.CourseId,
                x.SubjectId
            })
            .IsUnique();



        // =====================================================
        // ASSIGNMENTS
        // =====================================================

        builder.Entity<Assignment>()
            .ToTable("Assignments");

        builder.Entity<Assignment>()
            .HasKey(x => x.Id);


        // -----------------------------------------------------
        // Teacher
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);


        // -----------------------------------------------------
        // Academic Year
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasOne(x => x.AcademicYear)
            .WithMany()
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);


        // -----------------------------------------------------
        // Class
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasOne(x => x.Class)
            .WithMany()
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Restrict);


        // -----------------------------------------------------
        // Section
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasOne(x => x.Section)
            .WithMany()
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.SetNull);


        // -----------------------------------------------------
        // Course
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);


        // -----------------------------------------------------
        // Subject
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);


        // -----------------------------------------------------
        // TeacherId
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.TeacherId)
            .HasMaxLength(450);


        // -----------------------------------------------------
        // Title
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(250);


        // -----------------------------------------------------
        // Description
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.Description)
            .HasMaxLength(5000);


        // -----------------------------------------------------
        // Status
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.IsPublished)
            .IsRequired()
            .HasMaxLength(30);


        // -----------------------------------------------------
        // Max Marks
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.MaximumMarks)
            .HasPrecision(10, 2);


        // -----------------------------------------------------
        // Date fields
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.Deadline)
            .HasColumnType("timestamp with time zone");

        builder.Entity<Assignment>()
            .Property(x => x.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Entity<Assignment>()
            .Property(x => x.UpdatedAt)
            .HasColumnType("timestamp with time zone");


        // -----------------------------------------------------
        // Audit fields
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .Property(x => x.CreatedBy)
            .HasMaxLength(450);

        builder.Entity<Assignment>()
            .Property(x => x.UpdatedBy)
            .HasMaxLength(450);


        // -----------------------------------------------------
        // Indexes
        // -----------------------------------------------------

        builder.Entity<Assignment>()
            .HasIndex(x => x.TeacherId);

        builder.Entity<Assignment>()
            .HasIndex(x => new
            {
                x.ClassId,
                x.CourseId,
                x.SubjectId
            });

        builder.Entity<Assignment>()
            .HasIndex(x => x.Deadline);
    }
}