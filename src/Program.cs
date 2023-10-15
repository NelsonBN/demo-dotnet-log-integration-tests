using Api;
using Api.Infrastructure;

var builder = WebApplication.CreateSlimBuilder(args);


builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .ConfigureOptions<MySQLOptions.Setup>()
    .AddDbContext<DatabaseContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapProductsEndpoints();

app.Run();
