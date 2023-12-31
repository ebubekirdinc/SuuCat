using System.Reflection;
using EventBus.Constants;
using Logging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachine;
using SagaOrchestrationStateMachine.DbContext;
using SagaOrchestrationStateMachine.StateInstances;
using SagaOrchestrationStateMachine.StateMachines;
using Serilog;
using Tracing;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
            {
                opt.AddDbContext<DbContext, StateMachineDbContext>((provider, builder) =>
                {
                    builder.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"),
                        m => { m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name); });
                });

                opt.ConcurrencyMode = ConcurrencyMode.Optimistic;
            });

            cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                configure.Host(hostContext.Configuration["RabbitMQUrl"], "/", host =>
                {
                    host.Username("user");
                    host.Password("password");
                });

                configure.ReceiveEndpoint(QueuesConsts.CreateOrderMessageQueueName, e => { e.ConfigureSaga<OrderStateInstance>(provider); });
            }));
        });

        services.AddDbContext<StateMachineDbContext>(options =>
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));

        services.AddOpenTelemetryTracing(hostContext.Configuration);
        
        services.AddHostedService<Worker>();
    })
    .UseSerilog(SeriLogger.Configure)
    .Build();

using (var scope = host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var orderDbContext = serviceProvider.GetRequiredService<StateMachineDbContext>();
    orderDbContext.Database.Migrate();
}

host.Run();