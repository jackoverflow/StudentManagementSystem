using Dapper;
using Microsoft.Data.Sqlite;
using StudentSystemApp.Models;
using System.Collections.Generic;
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
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            SELECT Id, StudentNumber, FullName, Course 
            FROM Students";

        var students = (await connection.QueryAsync<Student>(sql)).AsList();
        return students;
    }

    public async Task<string> CreateAsync(Student student)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO Students (StudentNumber, FullName, Course) 
            VALUES (@StudentNumber, @FullName, @Course);
            SELECT last_insert_rowid();";

        var id = await connection.ExecuteScalarAsync<string>(sql, student);
        return id ?? string.Empty;
    }

    public async Task DeleteAsync(string id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        const string sql = "DELETE FROM Students WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
