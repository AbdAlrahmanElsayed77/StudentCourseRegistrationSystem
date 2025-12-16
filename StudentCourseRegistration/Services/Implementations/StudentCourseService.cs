using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCourseRegistration.Models.Entities;
using StudentCourseRegistration.Models.ViewModels;
using StudentCourseRegistration.Repositories.Interfaces;
using StudentCourseRegistration.Services.Interfaces;

namespace StudentCourseRegistration.Services.Implementations
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentCourseService(
            IStudentCourseRepository studentCourseRepository,
            ICourseRepository courseRepository,
            UserManager<ApplicationUser> userManager)
        {
            _studentCourseRepository = studentCourseRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<StudentCourseViewModel>> GetStudentCoursesAsync(string studentId)
        {
            var registrations = await _studentCourseRepository.GetStudentCoursesAsync(studentId);

            return registrations.Select(sc => new StudentCourseViewModel
            {
                Id = sc.Id,
                StudentId = sc.StudentId,
                StudentName = sc.Student.FullName,
                StudentEmail = sc.Student.Email ?? "",
                CourseId = sc.CourseId,
                CourseName = sc.Course.Name,
                CourseNameAr = sc.Course.NameAr,
                CourseCode = sc.Course.CourseId,
                Credits = sc.Course.Credits,
                Semester = sc.Course.Semester,
                RegisteredAt = sc.RegisteredAt,
                Status = sc.Status,
                CanUnregister = sc.Status == "Active"
            });
        }

        public async Task<bool> RegisterForCourseAsync(string studentId, int courseId)
        {
            // Check if already registered
            var existingRegistration = await _studentCourseRepository.GetRegistrationAsync(studentId, courseId);
            if (existingRegistration != null)
                throw new Exception("You are already registered for this course");

            // Check if course exists and is active
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || !course.IsActive)
                throw new Exception("Course not found or not available");

            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId,
                RegisteredAt = DateTime.UtcNow,
                Status = "Active"
            };

            await _studentCourseRepository.RegisterAsync(studentCourse);
            return true;
        }

        public async Task<bool> UnregisterFromCourseAsync(int registrationId, string studentId)
        {
            var registration = await _studentCourseRepository.GetByIdAsync(registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            if (registration.StudentId != studentId)
                throw new Exception("Unauthorized");

            if (registration.Status != "Active")
                throw new Exception("Cannot unregister from a course that is not active");

            return await _studentCourseRepository.UnregisterAsync(registrationId);
        }

        public async Task<bool> IsStudentRegisteredAsync(string studentId, int courseId)
        {
            return await _studentCourseRepository.IsStudentRegisteredAsync(studentId, courseId);
        }

        public async Task<IEnumerable<StudentListViewModel>> GetAllStudentsAsync()
        {
            var students = await _userManager.Users
                .Where(u => u.Email != "admin@university.com")
                .ToListAsync();

            var result = new List<StudentListViewModel>();

            foreach (var student in students)
            {
                var roles = await _userManager.GetRolesAsync(student);
                if (roles.Contains("Student"))
                {
                    var coursesCount = (await _studentCourseRepository.GetStudentCoursesAsync(student.Id)).Count();

                    result.Add(new StudentListViewModel
                    {
                        Id = student.Id,
                        FullName = student.FullName,
                        Email = student.Email ?? "",
                        PhoneNumber = student.PhoneNumber,
                        AcademicYear = student.AcademicYear,
                        RegisteredCoursesCount = coursesCount,
                        CreatedAt = student.CreatedAt
                    });
                }
            }

            return result.OrderBy(s => s.FullName);
        }

        public async Task<IEnumerable<StudentCourseViewModel>> GetCourseRegistrationsAsync(int courseId)
        {
            var registrations = await _studentCourseRepository.GetCourseRegistrationsAsync(courseId);

            return registrations.Select(sc => new StudentCourseViewModel
            {
                Id = sc.Id,
                StudentId = sc.StudentId,
                StudentName = sc.Student.FullName,
                StudentEmail = sc.Student.Email ?? "",
                CourseId = sc.CourseId,
                CourseName = sc.Course.Name,
                CourseNameAr = sc.Course.NameAr,
                CourseCode = sc.Course.CourseId,
                Credits = sc.Course.Credits,
                Semester = sc.Course.Semester,
                RegisteredAt = sc.RegisteredAt,
                Status = sc.Status
            });
        }
    }
}