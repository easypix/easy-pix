using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using server.Application.Services.Classes;
using server.Application.Services.Interfaces;
using server.WebApi.Protos;

var builder = WebApplication.CreateBuilder(args);

//Project path configuration
var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
ArgumentNullException.ThrowIfNull(projectPath);
builder.Configuration.SetBasePath(projectPath);

builder.Configuration.AddJsonFile("appsettings.json", true, true);

builder.Services.AddControllers();

builder.Services.AddGrpc(options => {});

builder.Services.AddScoped<IFilesService, FilesService>();

/*string? parentProcessString = Environment.GetEnvironmentVariable("PARENT_PROCESS_ID");

if (!int.TryParse(parentProcessString, out int parentProcessId))
{
    Console.WriteLine("Invalid or missing parent process ID");
    Environment.Exit(0);
}
else
{
    //start monitoring a parent process
    var monitor = new ParentProcessMonitor(parentProcessId);
}*/
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapControllers();

app.MapGrpcService<FilesProtoService>();

app.UseHttpsRedirection();

app.Run();