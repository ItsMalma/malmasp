using Malmasp.Models;
using Malmasp.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Malmasp.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly PostgresqlOptions _postgresqlOptions;

    public DbSet<User> Users { get; set; } = default!; 

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<PostgresqlOptions> postgresqlOptions) : base(options)
    {
        _postgresqlOptions = postgresqlOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            $"Host={_postgresqlOptions.Host};Port={_postgresqlOptions.Port};Database={_postgresqlOptions.Database};Username={_postgresqlOptions.Username};Password={_postgresqlOptions.Password}"
        );
    }
}