using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HabCo.X9.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // This connection string is used only for design-time tooling.
        // The runtime connection string will be configured in the main application.
        optionsBuilder.UseSqlite("Data Source=hab_x9.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}