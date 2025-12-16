using System.ComponentModel.DataAnnotations;

namespace StudentCourseRegistration.Models.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        [Display(Name = "Course ID")]
        [StringLength(20)]
        public string CourseId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course name is required")]
        [Display(Name = "Course Name (English)")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم المقرر مطلوب")]
        [Display(Name = "Course Name (Arabic)")]
        [StringLength(200)]
        public string NameAr { get; set; } = string.Empty;

        [Display(Name = "Description (English)")]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Description (Arabic)")]
        [StringLength(1000)]
        public string? DescriptionAr { get; set; }

        [Required(ErrorMessage = "Credits are required")]
        [Display(Name = "Credits")]
        [Range(1, 6, ErrorMessage = "Credits must be between 1 and 6")]
        public int Credits { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        [Display(Name = "Semester")]
        [StringLength(50)]
        public string Semester { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
    }
}