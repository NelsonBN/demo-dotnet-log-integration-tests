using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure;

internal sealed class DatabaseContext : DbContext
{
    public DbSet<Product> Products { get; set; } = default!;

    private readonly MySQLOptions _options;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;

    public DatabaseContext(
        IOptions<MySQLOptions> options,
        ILoggerFactory loggerFactory,
        IConfiguration configuration)
    {
        _options = options.Value;
        _loggerFactory = loggerFactory;
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseMySQL(_options.ConnectionString, options =>
            {
                options.EnableRetryOnFailure(_options.MaxRetryCount);
                options.CommandTimeout(_options.ConnandTineout);
            })
            .UseLoggerFactory(_loggerFactory)
            .EnableSensitiveDataLogging(_options.EnableSensitiveDataLogging)
            .EnableDetailedErrors(_options.EnableDetailedErrors);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder
            .Entity<Product>(product => product
                    .ToTable(nameof(Product))
                    .HasKey(product => product.Id));
}
