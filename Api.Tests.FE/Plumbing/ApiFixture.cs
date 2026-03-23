using Data;
using FastEndpoints.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Api.Tests.FE.Plumbing;

public class ApiFixture : AppFixture<Program>
{
    public string Schema { get; private set; } = null!;

    protected override async ValueTask PreSetupAsync()
    {
        await TestDatabase.StartAsync();

        Schema = TestSchema.NewSchemaName();
        await TestSchema.CreateAsync(TestDatabase.Instance.GetConnectionString(), Schema);
    }

    protected override void ConfigureApp(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:DefaultConnection", $"{TestDatabase.Instance.GetConnectionString()};Search Path={Schema}");

        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"]  =
                    $"{TestDatabase.Instance.GetConnectionString()};Search Path={Schema}"
            };

            cfg.AddInMemoryCollection(settings);
        });
    }

    protected override async ValueTask SetupAsync()
    {
        using var scope = Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    protected override async ValueTask TearDownAsync()
    {
        await TestSchema.DropAsync(TestDatabase.Instance.GetConnectionString(), Schema);
    }

}