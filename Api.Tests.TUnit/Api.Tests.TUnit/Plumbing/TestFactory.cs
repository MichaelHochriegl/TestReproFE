using Microsoft.AspNetCore.Hosting;
using TUnit.AspNetCore;

namespace Api.Tests.TUnit.Plumbing;

public class TestFactory : TestWebApplicationFactory<Program>
{
    [ClassDataSource<TestDatabase>(Shared = SharedType.PerTestSession)]
    public TestDatabase Database { get; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}