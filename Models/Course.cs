namespace StudentSystemApp.Models;

using System.ComponentModel.DataAnnotations;

public class Course
{
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Course description is required")]
    public string CourseDescription { get; set; } = string.Empty;
}