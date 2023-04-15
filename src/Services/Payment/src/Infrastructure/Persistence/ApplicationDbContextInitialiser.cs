using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace Payment.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    // private readonly UserManager<ApplicationUser> _userManager;
    // private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)//, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        // _userManager = userManager;
        // _roleManager = roleManager;
    }

    public void MigrateDatabaseAndSeed()
    {
        _logger.LogInformation("MigrateDatabaseAndSeedAsync started");
        try
        {
            if (_context.Database.IsNpgsql())
            {
                var retryPolicy = Policy.Handle<Exception>()
                    .WaitAndRetry(
                        retryCount: 5,
                        // 2 secs, 4, 8, 16, 32 
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, retryCount, context) =>
                        {
                            _logger.LogError("Retrying MigrateDatabaseAndSeed {RetryCount} of {ContextPolicyKey} at {ContextOperationKey}, due to: {Exception}", retryCount, context.PolicyKey,
                                context.OperationKey, exception);
                        });

                retryPolicy.Execute(MigrateAndSeed);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }

        _logger.LogInformation("MigrateDatabaseAndSeedAsync completed");
    }

    private void MigrateAndSeed()
    {
        _context.Database.Migrate();
        SeedDatabase();
    }

    public void SeedDatabase()
    {
        // Default roles
        // var administratorRole = new IdentityRole("Administrator");
        //
        // if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        // {
        //     await _roleManager.CreateAsync(administratorRole);
        // }
        //
        // // Default users
        // var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
        //
        // if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        // {
        //     await _userManager.CreateAsync(administrator, "Administrator1!");
        //     await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
        // }
        //
        // // Default data
        // // Seed, if necessary
        // if (!_context.TodoLists.Any())
        // {
        //     _context.TodoLists.Add(new TodoList
        //     {
        //         Title = "Todo List",
        //         Items =
        //         {
        //             new TodoItem { Title = "Make a todo list 📃" },
        //             new TodoItem { Title = "Check off the first item ✅" },
        //             new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
        //             new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
        //         }
        //     });
        //
        //     await _context.SaveChangesAsync();
        // }
    }
}
