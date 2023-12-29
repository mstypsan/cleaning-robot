using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Repositories;

public static class DatabaseServiceExtensions
{
    public static void ConfigureDatabase(this IServiceCollection services, string connectString)
    {
        services.AddDbContext<ExecutionsContext>(options =>
            options.UseNpgsql(connectString));

        services.AddNpgsql<ExecutionsContext>(connectString);

        services.AddScoped<IExecutionRepository, ExecutionsRepository>();
    }
}
