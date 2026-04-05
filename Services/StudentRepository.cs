using Microsoft.Data.Sqlite;
using StudentSystemApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSystemApp.Services;

public class StudentRepository
{
    private readonly string _dbPath = "students.db";

    public StudentRepository()
    {
        // Database initialization handled by DbInitializer
    }

    public async Task<List<Student>> GetAllAsync()
    {
        var students = new List<Student>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = @"
            SELECT Id, StudentNumber, FullName, Course 
            FROM Students";

        using var reader = await selectCmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(new Student
            {
                Id = reader.GetString(0),
                StudentNumber = reader.GetString(1),
                FullName = reader.GetString(2),
                Course = reader.GetString(3)
            });
        }

        return students;
    }
}
