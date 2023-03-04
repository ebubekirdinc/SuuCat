using MassTransit;
using SagaOrchestrationStateMachine.StateInstances;
using Shared.Constants;
using Shared.Events;
using Shared.Interfaces;
using Shared.Messages;

namespace SagaOrchestrationStateMachine.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
    public Event<IStockReservedEvent> StockReservedEvent { get; set; }
    public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
    public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }

    public State OrderCreated { get; private set; }
    public State StockReserved { get; private set; }
    public State StockNotReserved { get; private set; }
    public State PaymentCompleted { get; private set; }
    public State PaymentFailed { get; private set; }

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));

        Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Initially(
            When(OrderCreatedRequestEvent)
                .Then(context =>
                {
                    context.Saga.CustomerId = context.Message.CustomerId;
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                    context.Saga.PaymentAccountId = context.Message.PaymentAccountId;
                    context.Saga.TotalPrice = context.Message.TotalPrice;
                })
                .Then(context => { Console.WriteLine($"Before OrderCreatedRequestEvent : {context.Saga}"); })
                .Publish(
                    context => new OrderCreatedEvent(context.Saga.CorrelationId)
                    {
                        OrderItems = context.Message.OrderItems
                    })
                .TransitionTo(OrderCreated)
                .Then(context => { Console.WriteLine($"After OrderCreatedRequestEvent : {context.Saga}"); }));

        During(OrderCreated,
            When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{QueuesConsts.PaymentStockReservedRequestQueueName}"),
                    context => new StockReservedRequestPayment(context.Saga.CorrelationId)
                    {
                        OrderItems = context.Message.OrderItems,
                        TotalPrice = context.Saga.TotalPrice,
                        CustomerId = context.Saga.CustomerId
                    })
                .Then(context => { Console.WriteLine($"After StockReservedEvent : {context.Saga}"); }),
            When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Publish(
                    context => new OrderRequestFailedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        Reason = context.Message.Reason
                    })
                .Then(context => { Console.WriteLine($"After StockReservedEvent : {context.Saga}"); })
        );

        During(StockReserved,
            When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(
                    context => new OrderRequestCompletedEvent
                    {
                        OrderId = context.Saga.OrderId
                    })
                .Then(context => { Console.WriteLine($"After PaymentCompletedEvent : {context.Saga}"); })
                .Finalize(),
            When(PaymentFailedEvent)
                .Publish(context => new OrderRequestFailedEvent() { OrderId = context.Saga.OrderId, Reason = context.Message.Reason })
                .Send(new Uri($"queue:{QueuesConsts.StockRollBackMessageQueueName}"),
                    context => new StockRollbackMessage
                    {
                        OrderItems = context.Message.OrderItems
                    })
                .TransitionTo(PaymentFailed)
                .Then(context => { Console.WriteLine($"After PaymentFailedEvent : {context.Saga}"); })
        );
    }
}