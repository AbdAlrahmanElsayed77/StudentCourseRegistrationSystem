using StudentCourseRegistration.Models.Entities;

namespace StudentCourseRegistration.Repositories.Interfaces
{
    public interface IStudentCourseRepository
    {
        Task<IEnumerable<StudentCourse>> GetStudentCoursesAsync(string studentId);
        Task<StudentCourse?> GetByIdAsync(int id);
        Task<StudentCourse?> GetRegistrationAsync(string studentId, int courseId);
        Task<StudentCourse> RegisterAsync(StudentCourse studentCourse);
        Task<bool> UnregisterAsync(int id);
        Task<bool> IsStudentRegisteredAsync(string studentId, int courseId);
        Task<IEnumerable<StudentCourse>> GetCourseRegistrationsAsync(int courseId);
        Task<int> GetCourseRegistrationsCountAsync(int courseId);
        Task<IEnumerable<StudentCourse>> GetAllRegistrationsAsync();
    }
}