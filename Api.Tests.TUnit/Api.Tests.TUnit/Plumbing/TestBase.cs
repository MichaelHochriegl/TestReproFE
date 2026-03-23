using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TUnit.AspNetCore;

namespace Api.Tests.TUnit.Plumbing;

public class TestBase : WebApplicationTest<TestFactory, Program>
{
    [ClassDataSource<TestDatabase>(Shared = SharedType.PerTestSession)]
    public TestDatabase Database { get; init; } = null!;

    private string _schemaName = "public";
    private string _connectionString = "";

    protected override async Task SetupAsync()
    {
        _schemaName = GetIsolatedName("schema");

        var adminConnectionString = Database.Container.GetConnectionString();

        await using (var connection = new NpgsqlConnection(adminConnectionString))
        {
            await connection.OpenAsync();
            
            await using var cmd = connection.CreateCommand();
            cmd.CommandText = $"CREATE SCHEMA IF NOT EXISTS {_schemaName}";
            await cmd.ExecuteNonQueryAsync();
        }
        
        _connectionString = new NpgsqlConnectionStringBuilder(adminConnectionString)
        {
            SearchPath = _schemaName
        }.ConnectionString;

        var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(_connectionString).Options;
        
        await using var dbContext = new AppDbContext(options);
        await dbContext.Database.MigrateAsync();
    }

    protected override void ConfigureTestConfiguration(IConfigurationBuilder config)
    {
        config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = _connectionString
        });
    }
    
    [After(Test)]
    public async Task CleanupAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = $"DROP SCHEMA IF EXISTS {_schemaName} CASCADE";
        await cmd.ExecuteNonQueryAsync();
    }
}