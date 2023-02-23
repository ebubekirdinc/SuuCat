using System.Text;
using MassTransit;

namespace SagaOrchestrationStateMachine.StateInstances;

public class OrderStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }
    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public string PaymentAccountId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedDate { get; set; }

    public override string ToString()
    {
        var properties = GetType().GetProperties();

        var sb = new StringBuilder();

        properties.ToList().ForEach(p =>
        {
            var value = p.GetValue(this, null);
            sb.AppendLine($"{p.Name}:{value}");
        });

        sb.Append("------------------------");
        return sb.ToString();
    }
}