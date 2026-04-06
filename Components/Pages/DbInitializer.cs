using Microsoft.Data.Sqlite;
using System.IO;
using StudentSystemApp.Models;

namespace StudentSystemApp.Components.Pages;

public static class DbInitializer
{
    public static void Initialize()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db");

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
        
        // Enable foreign key constraints
        using var pragmaCmd = new SqliteCommand("PRAGMA foreign_keys = ON;", connection);
        pragmaCmd.ExecuteNonQuery();

        var command = connection.CreateCommand();

        // Create Courses table
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Courses (
                CourseId INTEGER PRIMARY KEY AUTOINCREMENT,
                CourseDescription TEXT NOT NULL
            )";
        command.ExecuteNonQuery();

        // Create Students table with Foreign Key
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id TEXT PRIMARY KEY,
                StudentNumber TEXT NOT NULL UNIQUE,
                FullName TEXT NOT NULL,
                CourseId INTEGER,
                FOREIGN KEY(CourseId) REFERENCES Courses(CourseId) ON DELETE SET NULL
            )";
        command.ExecuteNonQuery();

        // SCHEMA MIGRATION WORKAROUND:
        // This logic handles existing 'students.db' files from Version 1 of the application.
        // Version 1 used a simple text field for courses, while Version 2 uses a relational 'CourseId'.
        // Instead of forcing users to delete their data, we "sniff" the table schema using PRAGMA.
        // If 'CourseId' is missing, we perform an 'ALTER TABLE' to add the column and maintain 
        // backward compatibility without crashing the app on startup.
        command.CommandText = "PRAGMA table_info(Students);";
        var hasCourseId = false;
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                if (reader["name"].ToString() == "CourseId")
                {
                    hasCourseId = true;
                }
            }
        }

        if (!hasCourseId)
        {
            command.CommandText = "ALTER TABLE Students ADD COLUMN CourseId INTEGER REFERENCES Courses(CourseId);";
            command.ExecuteNonQuery();
        }

        // Seed Courses if empty
        command.CommandText = "SELECT COUNT(*) FROM Courses";
        var courseCount = (long)(command.ExecuteScalar() ?? 0);
        if (courseCount == 0)
        {
            string[] descriptions = { "Computer Science", "Information Technology", "Software Engineering", "Data Science" };
            foreach (var desc in descriptions)
            {
                command.CommandText = "INSERT INTO Courses (CourseDescription) VALUES (@desc)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@desc", desc);
                command.ExecuteNonQuery();
            }
        }

        // Insert sample data only if no STU00* records exist (prevents duplicates)
        command.CommandText = "SELECT COUNT(*) FROM Students WHERE StudentNumber LIKE 'STU00%'";
        var sampleCountObj = command.ExecuteScalar();
        var sampleCount = sampleCountObj != null ? (long)sampleCountObj : 0;

        if (sampleCount == 0)
        {
            var students = new[]
            {
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU001", FullName = "John Doe", CourseId = 1 },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU002", FullName = "Jane Smith", CourseId = 2 },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU003", FullName = "Bob Johnson", CourseId = 3 },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU004", FullName = "Alice Brown", CourseId = 4 }
            };

            foreach (var student in students)
            {
                command.CommandText = @"
                    INSERT INTO Students (Id, StudentNumber, FullName, CourseId)
                    VALUES (@id, @studentNumber, @fullName, @courseId)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", student.Id);
                command.Parameters.AddWithValue("@studentNumber", student.StudentNumber);
                command.Parameters.AddWithValue("@fullName", student.FullName);
                command.Parameters.AddWithValue("@courseId", student.CourseId);
                command.ExecuteNonQuery();
            }
        }
    }
}
