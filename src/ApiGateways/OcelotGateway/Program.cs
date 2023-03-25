using Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);
builder.Services.AddOcelot();
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName.ToLower()}.json");
    
 

var app = builder.Build();

await app.UseOcelot();

app.Run();