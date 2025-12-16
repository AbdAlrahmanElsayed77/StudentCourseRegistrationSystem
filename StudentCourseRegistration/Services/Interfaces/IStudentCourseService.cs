using StudentCourseRegistration.Models.ViewModels;

namespace StudentCourseRegistration.Services.Interfaces
{
    public interface IStudentCourseService
    {
        Task<IEnumerable<StudentCourseViewModel>> GetStudentCoursesAsync(string studentId);
        Task<bool> RegisterForCourseAsync(string studentId, int courseId);
        Task<bool> UnregisterFromCourseAsync(int registrationId, string studentId);
        Task<bool> IsStudentRegisteredAsync(string studentId, int courseId);
        Task<IEnumerable<StudentListViewModel>> GetAllStudentsAsync();
        Task<IEnumerable<StudentCourseViewModel>> GetCourseRegistrationsAsync(int courseId);
    }
}