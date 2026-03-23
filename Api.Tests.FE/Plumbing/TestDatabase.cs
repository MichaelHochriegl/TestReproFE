using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace Api.Tests.FE.Plumbing;

public static class TestDatabase
{
    private static readonly Lazy<PostgreSqlContainer> Container = new(() =>
        new PostgreSqlBuilder("postgres:18-alpine")
            .WithDatabase("test")
            .WithUsername("my-user")
            .WithPassword("bar")
            .Build());

    public static PostgreSqlContainer Instance => Container.Value;

    public static async Task StartAsync()
    {
        if (Instance.State != TestcontainersStates.Running)
            await Instance.StartAsync();
    }
}