namespace Assessment.Application.IntegrationTests;

internal class CustomWebApplicationFactory //: WebApplicationFactory<Program>
{
    // protected override void ConfigureWebHost(IWebHostBuilder builder)
    // {
    //     builder.ConfigureAppConfiguration(configurationBuilder =>
    //     {
    //         var integrationConfig = new ConfigurationBuilder()
    //             .AddJsonFile("appsettings.json")
    //             .AddEnvironmentVariables()
    //             .Build();
    //
    //         configurationBuilder.AddConfiguration(integrationConfig);
    //     });
    //
    //     builder.ConfigureServices((builder, services) =>
    //     {
    //         services
    //             .Remove<ICurrentUserService>()
    //             .AddTransient(provider => Mock.Of<ICurrentUserService>(s =>
    //                 s.UserId == GetCurrentUserId()));
    //
    //         services
    //             .Remove<DbContextOptions<ApplicationDbContext>>()
    //             .AddDbContext<ApplicationDbContext>((sp, options) =>
    //                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    //                     builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    //     });
    // }
}
