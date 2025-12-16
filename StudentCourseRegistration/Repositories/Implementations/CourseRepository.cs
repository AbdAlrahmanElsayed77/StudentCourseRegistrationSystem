using Microsoft.EntityFrameworkCore;
using StudentCourseRegistration.Data;
using StudentCourseRegistration.Models.Entities;
using StudentCourseRegistration.Repositories.Interfaces;

namespace StudentCourseRegistration.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .OrderBy(c => c.CourseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .OrderBy(c => c.CourseId)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.StudentCourses)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetByCourseIdAsync(string courseId)
        {
            return await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<Course> CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await GetByIdAsync(id);
            if (course == null)
                return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CourseIdExistsAsync(string courseId, int? excludeId = null)
        {
            return await _context.Courses
                .AnyAsync(c => c.CourseId == courseId && (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task<bool> HasRegistrationsAsync(int courseId)
        {
            return await _context.StudentCourses
                .AnyAsync(sc => sc.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            searchTerm = searchTerm.ToLower();

            return await _context.Courses
                .Where(c => c.CourseId.ToLower().Contains(searchTerm) ||
                           c.Name.ToLower().Contains(searchTerm) ||
                           c.NameAr.Contains(searchTerm) ||
                           c.Semester.ToLower().Contains(searchTerm))
                .OrderBy(c => c.CourseId)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Courses.CountAsync();
        }
    }
}