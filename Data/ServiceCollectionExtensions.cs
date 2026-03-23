using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDatabase(string connectionString)
        {
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));
            
            return services;
        }
    }
}