using Api.Infrastructure;
using Api.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace Integration.Tests.Config;

public sealed class IntegrationTestsFactory : WebApplicationFactory<ProductRequest>, IAsyncLifetime
{
    private const string DB_IMAGE = "mariadb:10.5.8";
    private const string DB_DATABASE = "demo";
    private const string DB_USERNAME = "root";
    private const string DB_ROOT_PASSWORD = "testpassword";
    private const int DB_CONTAINER_PORT = 3306;

    private readonly IContainer _dbContainer;
    public ITestOutputHelper Output { get; set; } = default!;

    public IntegrationTestsFactory()
        => _dbContainer = new ContainerBuilder()
            .WithImage(DB_IMAGE)
            .WithEnvironment("MYSQL_DATABASE", DB_DATABASE)
            .WithEnvironment("MYSQL_ROOT_PASSWORD", DB_ROOT_PASSWORD)
            .WithPortBinding(DB_CONTAINER_PORT, true)
            .WithResourceMapping(Path.GetFullPath("./Data"), "/docker-entrypoint-initdb.d")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(DB_CONTAINER_PORT))
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Error);
                logging.AddFilter(_ => true);
                logging.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(Output));
            })
            .ConfigureTestServices(services =>
                services.AddSingleton<IOptions<MySQLOptions>>(new OptionsWrapper<MySQLOptions>(new MySQLOptions
                {
                    MaxRetryCount = 1,
                    ConnandTineout = 20,

                    Database = DB_DATABASE,
                    Host = _dbContainer.Hostname,
                    Port = _dbContainer.GetMappedPublicPort(DB_CONTAINER_PORT),
                    Username = DB_USERNAME,
                    Password = DB_ROOT_PASSWORD,

                    EnableSensitiveDataLogging = true,
                    EnableDetailedErrors = true,
                })));



    public async Task InitializeAsync()
        => await _dbContainer.StartAsync().ConfigureAwait(false);

    public new async Task DisposeAsync()
        => await _dbContainer.StopAsync().ConfigureAwait(false);
}

public abstract class IntegrationTests : IClassFixture<IntegrationTestsFactory> { }
