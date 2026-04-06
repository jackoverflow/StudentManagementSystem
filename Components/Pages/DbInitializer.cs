using Microsoft.Data.Sqlite;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using StudentSystemApp.Models;

namespace StudentSystemApp.Components.Pages;

public static class DbInitializer
{
    public static void Initialize()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db");

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

        // SCHEMA RECOVERY WORKAROUND:
        // This logic handles the "downgrade" scenario where a user switches from the Relational branch (Version 2)
        // back to this Simple branch (Version 1).
        // In Version 2, the 'Course' column might have been replaced or modified.
        // To prevent a crash, we "sniff" the table schema using PRAGMA. If the 'Course' column 
        // is missing, we re-add it to maintain backward compatibility with this branch's data model.
        command.CommandText = "PRAGMA table_info(Students);";
        var columns = new List<string>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                columns.Add(reader["name"].ToString() ?? "");
            }
        }

        if (!columns.Any(c => string.Equals(c, "Course", StringComparison.OrdinalIgnoreCase)))
        {
            command.CommandText = "ALTER TABLE Students ADD COLUMN Course TEXT NOT NULL DEFAULT ''";
            command.ExecuteNonQuery();
        }

        // Insert sample data only if no STU00* records exist (prevents duplicates)
        command.CommandText = "SELECT COUNT(*) FROM Students WHERE StudentNumber LIKE 'STU00%'";
        var sampleCountObj = command.ExecuteScalar();
        var sampleCount = sampleCountObj != null ? (long)sampleCountObj : 0;

        if (sampleCount == 0)
        {
            using var transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            var students = new[]
            {
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU001", FullName = "John Doe", Course = "Computer Science" },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU002", FullName = "Jane Smith", Course = "Information Technology" },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU003", FullName = "Bob Johnson", Course = "Software Engineering" },
                new Student { Id = Guid.NewGuid().ToString(), StudentNumber = "STU004", FullName = "Alice Brown", Course = "Data Science" }
            };

            try
            {
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
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
