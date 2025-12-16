using StudentCourseRegistration.Models.Entities;
using StudentCourseRegistration.Models.ViewModels;
using StudentCourseRegistration.Repositories.Interfaces;
using StudentCourseRegistration.Services.Interfaces;

namespace StudentCourseRegistration.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;

        public CourseService(
            ICourseRepository courseRepository,
            IStudentCourseRepository studentCourseRepository)
        {
            _courseRepository = courseRepository;
            _studentCourseRepository = studentCourseRepository;
        }

        public async Task<IEnumerable<CourseViewModel>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(MapToViewModel);
        }

        public async Task<IEnumerable<AvailableCourseViewModel>> GetAvailableCoursesAsync(string studentId)
        {
            var courses = await _courseRepository.GetActiveCoursesAsync();
            var result = new List<AvailableCourseViewModel>();

            foreach (var course in courses)
            {
                var isRegistered = await _studentCourseRepository.IsStudentRegisteredAsync(studentId, course.Id);
                var registrationsCount = await _studentCourseRepository.GetCourseRegistrationsCountAsync(course.Id);

                result.Add(new AvailableCourseViewModel
                {
                    Id = course.Id,
                    CourseId = course.CourseId,
                    Name = course.Name,
                    NameAr = course.NameAr,
                    Description = course.Description,
                    DescriptionAr = course.DescriptionAr,
                    Credits = course.Credits,
                    Semester = course.Semester,
                    IsRegistered = isRegistered,
                    RegisteredStudentsCount = registrationsCount
                });
            }

            return result;
        }

        public async Task<CourseViewModel?> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course == null ? null : MapToViewModel(course);
        }

        public async Task<CourseViewModel> CreateCourseAsync(CourseViewModel model)
        {
            var course = new Course
            {
                CourseId = model.CourseId,
                Name = model.Name,
                NameAr = model.NameAr,
                Description = model.Description,
                DescriptionAr = model.DescriptionAr,
                Credits = model.Credits,
                Semester = model.Semester,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _courseRepository.CreateAsync(course);
            return MapToViewModel(created);
        }

        public async Task<CourseViewModel> UpdateCourseAsync(CourseViewModel model)
        {
            var course = await _courseRepository.GetByIdAsync(model.Id);
            if (course == null)
                throw new Exception("Course not found");

            course.CourseId = model.CourseId;
            course.Name = model.Name;
            course.NameAr = model.NameAr;
            course.Description = model.Description;
            course.DescriptionAr = model.DescriptionAr;
            course.Credits = model.Credits;
            course.Semester = model.Semester;
            course.IsActive = model.IsActive;

            var updated = await _courseRepository.UpdateAsync(course);
            return MapToViewModel(updated);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            // Check if course has registrations
            var hasRegistrations = await _courseRepository.HasRegistrationsAsync(id);
            if (hasRegistrations)
                throw new Exception("Cannot delete course with existing registrations");

            return await _courseRepository.DeleteAsync(id);
        }

        public async Task<bool> CourseIdExistsAsync(string courseId, int? excludeId = null)
        {
            return await _courseRepository.CourseIdExistsAsync(courseId, excludeId);
        }

        public async Task<IEnumerable<CourseViewModel>> SearchCoursesAsync(string searchTerm)
        {
            var courses = await _courseRepository.SearchAsync(searchTerm);
            return courses.Select(MapToViewModel);
        }

        private CourseViewModel MapToViewModel(Course course)
        {
            return new CourseViewModel
            {
                Id = course.Id,
                CourseId = course.CourseId,
                Name = course.Name,
                NameAr = course.NameAr,
                Description = course.Description,
                DescriptionAr = course.DescriptionAr,
                Credits = course.Credits,
                Semester = course.Semester,
                IsActive = course.IsActive,
                CreatedAt = course.CreatedAt
            };
        }
    }
}