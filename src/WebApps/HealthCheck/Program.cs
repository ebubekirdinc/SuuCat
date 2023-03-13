var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
            
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();
 
var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecksUI();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});   

app.Run();

