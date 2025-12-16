namespace StudentCourseRegistration.Models.ViewModels
{
    public class StudentCourseViewModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseNameAr { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Semester { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; }
        public string Status { get; set; } = string.Empty;

        public bool CanUnregister { get; set; } = true;
    }
}