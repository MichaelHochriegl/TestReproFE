using Npgsql;

namespace Api.Tests.FE.Plumbing;

public static class TestSchema
{
    public static string NewSchemaName() => $"test_{Guid.NewGuid():N}";

    public static async Task CreateAsync(string connectionString, string schema)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"""CREATE SCHEMA {schema};""";
        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task DropAsync(string connectionString, string schema)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"""DROP SCHEMA IF EXISTS {schema} CASCADE;""";
        await cmd.ExecuteNonQueryAsync();
    }
}