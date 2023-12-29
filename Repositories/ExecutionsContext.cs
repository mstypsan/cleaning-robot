using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class ExecutionsContext(DbContextOptions<ExecutionsContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Execution> Executions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseNpgsql()
        .UseSnakeCaseNamingConvention();
}

