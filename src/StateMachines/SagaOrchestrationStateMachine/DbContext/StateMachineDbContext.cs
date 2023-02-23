using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachine.StateMaps;

namespace SagaOrchestrationStateMachine.DbContext;

public class StateMachineDbContext: SagaDbContext
{
    public StateMachineDbContext(DbContextOptions<StateMachineDbContext> options) : base(options)
    {
    }



    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new StateMachineMap(); }
    }
}