using Microsoft.AspNetCore.Identity;
using StudentCourseRegistration.Models.Entities;

namespace StudentCourseRegistration.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Starting to seed roles...");

            // Create Roles
            string[] roleNames = { "Admin", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Role '{roleName}' created successfully.");
                    }
                    else
                    {
                        logger.LogError($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.LogInformation($"Role '{roleName}' already exists.");
                }
            }

            // Create Default Admin User
            var adminEmail = "admin@university.com";
            logger.LogInformation($"Checking for admin user: {adminEmail}");

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                logger.LogInformation("Admin user not found. Creating...");

                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true, 
                    PhoneNumber = "01000000000",
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    LockoutEnabled = false 
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    logger.LogInformation("Admin user created successfully.");

                    // Add to Admin Role
                    var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation("Admin role assigned successfully.");
                    }
                    else
                    {
                        logger.LogError($"Failed to assign Admin role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                logger.LogInformation("Admin user already exists.");

                // Email confirmed
                if (!adminUser.EmailConfirmed)
                {
                    adminUser.EmailConfirmed = true;
                    await userManager.UpdateAsync(adminUser);
                    logger.LogInformation("Admin email confirmed.");
                }
            }

            // Seed Sample Courses
            await SeedCourses(serviceProvider, logger);
        }

        private static async Task SeedCourses(IServiceProvider serviceProvider, ILogger logger)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            logger.LogInformation("Checking for existing courses...");

            if (!context.Courses.Any())
            {
                logger.LogInformation("No courses found. Seeding sample courses...");

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

                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();

                logger.LogInformation($"{courses.Count} courses seeded successfully.");
            }
            else
            {
                logger.LogInformation("Courses already exist. Skipping seeding.");
            }
        }
    }
}