using Dapper;
using Microsoft.Data.Sqlite;
using StudentSystemApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StudentSystemApp.Services;

public class StudentRepository
{
    private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "students.db");

    public StudentRepository()
    {
        // Database initialization handled by DbInitializer
    }

    public async Task<List<Student>> GetAllAsync()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            SELECT Id, StudentNumber, FullName, Course 
            FROM Students";

        var students = (await connection.QueryAsync<Student>(sql)).AsList();
        return students;
    }

    public async Task CreateAsync(Student student)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO Students (Id, StudentNumber, FullName, Course)
            VALUES (@Id, @StudentNumber, @FullName, @Course)";

        await connection.ExecuteAsync(sql, student);
    }

    public async Task DeleteAsync(string id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = "DELETE FROM Students WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<Student?> GetByIdAsync(string id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            SELECT Id, StudentNumber, FullName, Course
            FROM Students WHERE Id = @Id";

        return await connection.QuerySingleOrDefaultAsync<Student>(sql, new { Id = id });
    }

    public async Task UpdateAsync(Student student)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            UPDATE Students 
            SET StudentNumber = @StudentNumber, 
                FullName = @FullName, 
                Course = @Course
            WHERE Id = @Id";

        await connection.ExecuteAsync(sql, student);
    }
}
