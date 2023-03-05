using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.Common.Interfaces;
using Subscription.Application.Common.Interfaces.MassTransit;
using Subscription.Infrastructure.MassTransit;
using Subscription.Infrastructure.Persistence;
using Subscription.Infrastructure.Persistence.Interceptors;
using Subscription.Infrastructure.Services;
using MassTransit;
using Shared.Constants;
using Subscription.Infrastructure.Consumers;
using Subscription.Infrastructure.Consumers.Events;
using Subscription.Infrastructure.Consumers.Messages;

namespace Subscription.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("OrderDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddTransient<IDateTime, DateTimeService>();
        // services.AddTransient<IIdentityService, IdentityService>(); 

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedEventConsumer>();
            x.AddConsumer<StockRollBackMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQUrl"], "/", host =>
                {
                    host.Username("user");
                    host.Password("password");
                });

                cfg.ReceiveEndpoint(QueuesConsts.StockOrderCreatedEventQueueName, e =>
                {
                    e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                });

                cfg.ReceiveEndpoint(QueuesConsts.StockRollBackMessageQueueName, e =>
                {
                    e.ConfigureConsumer<StockRollBackMessageConsumer>(context);
                });
            });
        });
        
        services.AddScoped<IMassTransitService, MassTransitService>();
        
        return services;
    }
}
