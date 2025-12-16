using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentCourseRegistration.Models.Entities;

namespace StudentCourseRegistration.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Course Entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NameAr)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.DescriptionAr)
                    .HasMaxLength(1000);

                entity.Property(e => e.Semester)
                    .IsRequired()
                    .HasMaxLength(50);

                // Unique Index on CourseId
                entity.HasIndex(e => e.CourseId)
                    .IsUnique();

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            // Configure StudentCourse Entity
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.StudentId)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");

                // Relationship: Student -> StudentCourses
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.StudentCourses)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship: Course -> StudentCourses
                entity.HasOne(e => e.Course)
                    .WithMany(c => c.StudentCourses)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Prevent duplicate registrations (Composite Unique Index)
                entity.HasIndex(e => new { e.StudentId, e.CourseId })
                    .IsUnique()
                    .HasDatabaseName("IX_StudentCourse_Student_Course");
            });

            // Configure ApplicationUser Entity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AcademicYear)
                    .HasMaxLength(50);
            });
        }
    }
}