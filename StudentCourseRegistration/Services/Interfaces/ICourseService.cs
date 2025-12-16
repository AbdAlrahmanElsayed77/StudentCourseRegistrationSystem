using StudentCourseRegistration.Models.ViewModels;

namespace StudentCourseRegistration.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseViewModel>> GetAllCoursesAsync();
        Task<IEnumerable<AvailableCourseViewModel>> GetAvailableCoursesAsync(string studentId);
        Task<CourseViewModel?> GetCourseByIdAsync(int id);
        Task<CourseViewModel> CreateCourseAsync(CourseViewModel model);
        Task<CourseViewModel> UpdateCourseAsync(CourseViewModel model);
        Task<bool> DeleteCourseAsync(int id);
        Task<bool> CourseIdExistsAsync(string courseId, int? excludeId = null);
        Task<IEnumerable<CourseViewModel>> SearchCoursesAsync(string searchTerm);
    }
}