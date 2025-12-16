using System.ComponentModel.DataAnnotations;

namespace StudentCourseRegistration.Models.Entities
{
    public class StudentCourse
    {
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Status { get; set; } = "Active"; 

        // Navigation Properties
        public virtual ApplicationUser Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
    }
}