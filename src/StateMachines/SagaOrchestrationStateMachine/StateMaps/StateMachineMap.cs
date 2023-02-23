using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaOrchestrationStateMachine.StateInstances;

namespace SagaOrchestrationStateMachine.StateMaps;

public class StateMachineMap: SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        // entity.Property(x => x.CustomerId).HasMaxLength(256);
    }
}