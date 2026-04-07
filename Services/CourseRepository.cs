using Microsoft.Data.Sqlite;
using Dapper;
using StudentSystemApp.Models;

namespace StudentSystemApp.Services;

public class CourseRepository
{
    private readonly string _dbPath;

    public CourseRepository()
    {
        // Points to the same database file defined in DbInitializer
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db");
    }

    public async Task<List<Course>> GetAllAsync()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        // SQLite and Dapper work together to map rows directly to your Course model
        var result = await connection.QueryAsync<Course>("SELECT * FROM Courses");
        return result.ToList();
    }
}