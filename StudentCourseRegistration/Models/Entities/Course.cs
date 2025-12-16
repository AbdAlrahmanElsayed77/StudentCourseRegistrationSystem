using System.ComponentModel.DataAnnotations;

namespace StudentCourseRegistration.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CourseId { get; set; } = string.Empty; 

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string NameAr { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(1000)]
        public string? DescriptionAr { get; set; }

        [Required]
        [Range(1, 6)]
        public int Credits { get; set; }

        [Required]
        [StringLength(50)]
        public string Semester { get; set; } = string.Empty; 

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}