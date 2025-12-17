using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistration.Services.Interfaces;

namespace StudentCourseRegistration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IStudentCourseService _studentCourseService;

        public AdminController(IStudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        // GET: Admin/Students
        public async Task<IActionResult> Students()
        {
            var students = await _studentCourseService.GetAllStudentsAsync();
            return View(students);
        }

        // GET: Admin/StudentCourses/5
        public async Task<IActionResult> StudentCourses(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return NotFound();
            }

            var courses = await _studentCourseService.GetStudentCoursesAsync(studentId);
            ViewBag.StudentId = studentId;
            return View(courses);
        }
    }
}