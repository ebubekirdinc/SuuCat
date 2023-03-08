using EventBus.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Common.Interfaces;
using Order.Application.Common.Interfaces.MassTransit;
using Order.Infrastructure.MassTransit;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Persistence.Interceptors;
using Order.Infrastructure.Services;
using MassTransit;
using Order.Infrastructure.Consumers;

namespace Order.Infrastructure;

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
            x.AddConsumer<OrderCompletedEventConsumer>();
            x.AddConsumer<OrderFailedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQUrl"], "/", host =>
                {
                    host.Username("user");
                    host.Password("password");
                });

                cfg.ReceiveEndpoint(QueuesConsts.OrderRequestCompletedEventtQueueName, x =>
                {
                    x.ConfigureConsumer<OrderCompletedEventConsumer>(context);
                });
                
                cfg.ReceiveEndpoint(QueuesConsts.OrderRequestFailedEventtQueueName, x =>
                {
                    x.ConfigureConsumer<OrderFailedEventConsumer>(context);
                });
            });
        });

        services.AddScoped<IMassTransitService, MassTransitService>();

        return services;
    }
}