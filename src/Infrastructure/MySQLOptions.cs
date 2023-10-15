using Microsoft.Extensions.Options;

namespace Api.Infrastructure;

public sealed record MySQLOptions
{
    public string? Host { get; set; }
    public string? Database { get; set; }
    public uint Port { get; set; } = 3306;
    public string? Username { get; set; }
    public string? Password { get; set; }


    private string? _connectionString;
    public string ConnectionString
        => _connectionString ??= $"server={Host}; Port={Port}; database={Database}; uid={Username}; password={Password};";

    public int MaxRetryCount { get; init; } = 3;
    public int ConnandTineout { get; init; } = 30;
    public bool EnableSensitiveDataLogging { get; init; }
    public bool EnableDetailedErrors { get; init; }



    internal sealed class Setup : IConfigureOptions<MySQLOptions>
    {
        private const string SECTION_NAME = "MySQL";

        private readonly IConfiguration _configuration;

        public Setup(IConfiguration configuration)
            => _configuration = configuration;

        public void Configure(MySQLOptions options)
            => _configuration.GetSection(SECTION_NAME)
                             .Bind(options);
    }
}
