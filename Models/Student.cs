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
    
    [Required(ErrorMessage = "Course is required")]
    public string Course { get; set; } = string.Empty;
}
