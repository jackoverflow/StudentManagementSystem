using Dapper;
using Microsoft.Data.Sqlite;
using StudentSystemApp.Models;
using System.IO;

namespace StudentSystemApp.Services;

public class CourseRepository
{
    private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db");

    public async Task<List<Course>> GetAllAsync()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = "SELECT CourseId, CourseDescription FROM Courses";
        var courses = (await connection.QueryAsync<Course>(sql)).AsList();
        return courses;
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = "SELECT CourseId, CourseDescription FROM Courses WHERE CourseId = @Id";
        return await connection.QuerySingleOrDefaultAsync<Course>(sql, new { Id = id });
    }

    public async Task CreateAsync(Course course)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO Courses (CourseDescription) 
            VALUES (@CourseDescription)";

        await connection.ExecuteAsync(sql, course);
    }

    public async Task UpdateAsync(Course course)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            UPDATE Courses 
            SET CourseDescription = @CourseDescription 
            WHERE CourseId = @CourseId";

        await connection.ExecuteAsync(sql, course);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = "DELETE FROM Courses WHERE CourseId = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}