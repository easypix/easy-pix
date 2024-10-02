using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using server.Application.Services.Classes;

var builder = WebApplication.CreateBuilder(args);

var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
ArgumentNullException.ThrowIfNull(projectPath);

builder.Configuration.SetBasePath(projectPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);

builder.Services.AddControllers();

builder.Services.AddScoped<FilesService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapControllers();

app.Run();