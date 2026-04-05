namespace StudentSystemApp.Models;

public class Student
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string StudentNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
}