using StudentCourseRegistration.Models.Entities;

namespace StudentCourseRegistration.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<IEnumerable<Course>> GetActiveCoursesAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course?> GetByCourseIdAsync(string courseId);
        Task<Course> CreateAsync(Course course);
        Task<Course> UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
        Task<bool> CourseIdExistsAsync(string courseId, int? excludeId = null);
        Task<bool> HasRegistrationsAsync(int courseId);
        Task<IEnumerable<Course>> SearchAsync(string searchTerm);
        Task<int> GetTotalCountAsync();
    }
}