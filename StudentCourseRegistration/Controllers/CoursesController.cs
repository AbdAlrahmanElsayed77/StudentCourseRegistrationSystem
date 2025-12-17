using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistration.Models.Entities;
using StudentCourseRegistration.Models.ViewModels;
using StudentCourseRegistration.Services.Interfaces;

namespace StudentCourseRegistration.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(
            ICourseService courseService,
            IStudentCourseService studentCourseService,
            UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _studentCourseService = studentCourseService;
            _userManager = userManager;
        }

        // GET: Courses (Admin Only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchTerm)
        {
            IEnumerable<CourseViewModel> courses;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                courses = await _courseService.SearchCoursesAsync(searchTerm);
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                courses = await _courseService.GetAllCoursesAsync();
            }

            return View(courses);
        }

        // GET: Courses/Create (Admin Only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if CourseId already exists
                if (await _courseService.CourseIdExistsAsync(model.CourseId))
                {
                    ModelState.AddModelError("CourseId", "This Course ID already exists.");
                    return View(model);
                }

                await _courseService.CreateCourseAsync(model);
                TempData["SuccessMessage"] = "Course created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Courses/Edit/5 (Admin Only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CourseViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if CourseId already exists (excluding current course)
                if (await _courseService.CourseIdExistsAsync(model.CourseId, model.Id))
                {
                    ModelState.AddModelError("CourseId", "This Course ID already exists.");
                    return View(model);
                }

                await _courseService.UpdateCourseAsync(model);
                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Courses/Delete/5 (Admin Only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Available (Student)
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Available()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var courses = await _courseService.GetAvailableCoursesAsync(userId);
            return View(courses);
        }

        // POST: Courses/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Register(int courseId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                await _studentCourseService.RegisterForCourseAsync(userId, courseId);
                TempData["SuccessMessage"] = "Successfully registered for the course!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Available));
        }

        // GET: Courses/MyCourses (Student)
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyCourses()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var courses = await _studentCourseService.GetStudentCoursesAsync(userId);
            return View(courses);
        }

        // POST: Courses/Unregister
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Unregister(int registrationId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                await _studentCourseService.UnregisterFromCourseAsync(registrationId, userId);
                TempData["SuccessMessage"] = "Successfully unregistered from the course!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(MyCourses));
        }

        // GET: Courses/Details/5 (Admin)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var registrations = await _studentCourseService.GetCourseRegistrationsAsync(id);
            ViewBag.Registrations = registrations;

            return View(course);
        }
    }
}