namespace StudentCourseRegistration.Models.ViewModels
{
    public class AvailableCourseViewModel
    {
        public int Id { get; set; }
        public string CourseId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }
        public int Credits { get; set; }
        public string Semester { get; set; } = string.Empty;
        public bool IsRegistered { get; set; }
        public int RegisteredStudentsCount { get; set; }
    }
}