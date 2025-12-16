using Microsoft.EntityFrameworkCore;
using StudentCourseRegistration.Data;
using StudentCourseRegistration.Models.Entities;
using StudentCourseRegistration.Repositories.Interfaces;

namespace StudentCourseRegistration.Repositories.Implementations
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentCourse>> GetStudentCoursesAsync(string studentId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .Where(sc => sc.StudentId == studentId)
                .OrderByDescending(sc => sc.RegisteredAt)
                .ToListAsync();
        }

        public async Task<StudentCourse?> GetByIdAsync(int id)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<StudentCourse?> GetRegistrationAsync(string studentId, int courseId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task<StudentCourse> RegisterAsync(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();
            return studentCourse;
        }

        public async Task<bool> UnregisterAsync(int id)
        {
            var registration = await GetByIdAsync(id);
            if (registration == null)
                return false;

            _context.StudentCourses.Remove(registration);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsStudentRegisteredAsync(string studentId, int courseId)
        {
            return await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task<IEnumerable<StudentCourse>> GetCourseRegistrationsAsync(int courseId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .Where(sc => sc.CourseId == courseId)
                .OrderBy(sc => sc.Student.FullName)
                .ToListAsync();
        }

        public async Task<int> GetCourseRegistrationsCountAsync(int courseId)
        {
            return await _context.StudentCourses
                .CountAsync(sc => sc.CourseId == courseId);
        }

        public async Task<IEnumerable<StudentCourse>> GetAllRegistrationsAsync()
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .OrderByDescending(sc => sc.RegisteredAt)
                .ToListAsync();
        }
    }
}