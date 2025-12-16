using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StudentCourseRegistration.Models.Entities;

namespace StudentCourseRegistration.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger logger)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            logger.LogInformation("Starting database seeding...");

            // Create Roles
            string[] roleNames = { "Admin", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    logger.LogInformation($"Role '{roleName}' created successfully.");
                }
                else
                {
                    logger.LogInformation($"Role '{roleName}' already exists.");
                }
            }

            // Create Default Admin User
            var adminEmail = "admin@university.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true,
                    PhoneNumber = "01000000000",
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    logger.LogInformation("Admin user created successfully.");
                }
                else
                {
                    logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                logger.LogInformation("Admin user already exists.");
            }

            // Seed Sample Courses
            await SeedCourses(context, logger);
            
            logger.LogInformation("Database seeding completed.");
        }

        private static async Task SeedCourses(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("Checking if courses exist...");
                var existingCoursesCount = context.Courses.Count();
                logger.LogInformation($"Existing courses count: {existingCoursesCount}");

                if (existingCoursesCount == 0)
                {
                    logger.LogInformation("No courses found. Adding sample courses...");

                    var courses = new List<Course>
                    {
                        new Course
                        {
                            CourseId = "CS101",
                            Name = "Introduction to Computer Science",
                            NameAr = "مقدمة في علوم الحاسب",
                            Description = "Basic concepts of computer science and programming",
                            DescriptionAr = "المفاهيم الأساسية لعلوم الحاسب والبرمجة",
                            Credits = 3,
                            Semester = "Fall 2024",
                            IsActive = true
                        },
                        new Course
                        {
                            CourseId = "MATH201",
                            Name = "Calculus I",
                            NameAr = "حساب التفاضل والتكامل 1",
                            Description = "Introduction to differential and integral calculus",
                            DescriptionAr = "مقدمة في حساب التفاضل والتكامل",
                            Credits = 4,
                            Semester = "Fall 2024",
                            IsActive = true
                        },
                        new Course
                        {
                            CourseId = "ENG102",
                            Name = "English Composition",
                            NameAr = "التحرير الإنجليزي",
                            Description = "Academic writing and composition skills",
                            DescriptionAr = "مهارات الكتابة الأكاديمية والتحرير",
                            Credits = 3,
                            Semester = "Fall 2024",
                            IsActive = true
                        },
                        new Course
                        {
                            CourseId = "PHY101",
                            Name = "Physics I",
                            NameAr = "فيزياء 1",
                            Description = "Mechanics and thermodynamics",
                            DescriptionAr = "الميكانيكا والديناميكا الحرارية",
                            Credits = 4,
                            Semester = "Spring 2025",
                            IsActive = true
                        },
                        new Course
                        {
                            CourseId = "CS201",
                            Name = "Data Structures",
                            NameAr = "هياكل البيانات",
                            Description = "Study of data structures and algorithms",
                            DescriptionAr = "دراسة هياكل البيانات والخوارزميات",
                            Credits = 3,
                            Semester = "Spring 2025",
                            IsActive = true
                        }
                    };

                    logger.LogInformation($"Adding {courses.Count} courses to database...");
                    await context.Courses.AddRangeAsync(courses);
                    
                    var savedCount = await context.SaveChangesAsync();
                    logger.LogInformation($"Successfully saved {savedCount} courses to database.");
                }
                else
                {
                    logger.LogInformation("Courses already exist in database. Skipping seed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while seeding courses.");
                throw;
            }
        }
    }
}