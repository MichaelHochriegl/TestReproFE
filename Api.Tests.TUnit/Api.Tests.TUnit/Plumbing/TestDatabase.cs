using Testcontainers.PostgreSql;
using TUnit.Core.Interfaces;

namespace Api.Tests.TUnit.Plumbing;

public class TestDatabase : IAsyncInitializer, IAsyncDisposable
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder("postgres:18-alpine")
        .WithDatabase("test")
        .WithUsername("my-user")
        .WithPassword("bar")
        .Build();
    
    public async Task InitializeAsync() => await Container.StartAsync();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}