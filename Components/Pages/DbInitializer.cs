using Microsoft.Data.Sqlite;
using StudentSystemApp.Models;

namespace StudentSystemApp.Components.Pages;

public static class DbInitializer
{
    public static void Initialize()
    {
        var dbPath = "students.db";

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();

        // Create Students table
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id TEXT PRIMARY KEY,
                StudentNumber TEXT NOT NULL UNIQUE,
                FullName TEXT NOT NULL,
                Course TEXT NOT NULL
            )";
        command.ExecuteNonQuery();

        // Insert sample data if table is empty
        command.CommandText = "SELECT COUNT(*) FROM Students";
        var countObj = command.ExecuteScalar();
        var count = countObj != null ? (long)countObj : 0;

        if (count == 0)
        {
            var students = new[]
            {
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU001", FullName = "John Doe", Course = "Computer Science" },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU002", FullName = "Jane Smith", Course = "Information Technology" },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU003", FullName = "Bob Johnson", Course = "Software Engineering" },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU004", FullName = "Alice Brown", Course = "Data Science" }
            };

            foreach (var student in students)
            {
                command.CommandText = @"
                    INSERT INTO Students (Id, StudentNumber, FullName, Course)
                    VALUES (@id, @studentNumber, @fullName, @course)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", student.Id);
                command.Parameters.AddWithValue("@studentNumber", student.StudentNumber);
                command.Parameters.AddWithValue("@fullName", student.FullName);
                command.Parameters.AddWithValue("@course", student.Course);
                command.ExecuteNonQuery();
            }
        }
    }
}
