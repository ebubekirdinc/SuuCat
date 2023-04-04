using Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);
 
builder.Services.AddAuthentication().AddJwtBearer("TestKey", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_ocelot";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot();  

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName.ToLower()}.json", true, true);
    
var app = builder.Build();

await app.UseOcelot();

app.Run();