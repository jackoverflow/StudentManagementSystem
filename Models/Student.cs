namespace StudentSystemApp.Models;

using System.ComponentModel.DataAnnotations;

public class Student
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required(ErrorMessage = "Student number is required")]
    [StringLength(10, ErrorMessage = "Max 10 chars")]
    public string StudentNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, ErrorMessage = "Max 100 chars")]
    public string FullName { get; set; } = string.Empty;
    
    // This is now a display-only property populated by the JOIN
    public string? Course { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid course")]
    public int CourseId { get; set; }
}
