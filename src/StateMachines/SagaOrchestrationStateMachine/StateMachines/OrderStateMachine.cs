using EventBus.Constants;
using EventBus.Events;
using EventBus.Events.Interfaces;
using EventBus.Messages;
using EventBus.Messages.Interfaces;
using MassTransit;
using SagaOrchestrationStateMachine.StateInstances;

namespace SagaOrchestrationStateMachine.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    private readonly ILogger<OrderStateMachine> _logger;

    // Commands
    private Event<ICreateOrderMessage> CreateOrderMessage { get; set; }

    // Events
    private Event<IStockReservedEvent> StockReservedEvent { get; set; }
    private Event<IStockReservationFailedEvent> StockReservationFailedEvent { get; set; }
    private Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    private Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }

    // States
    private State OrderCreated { get; set; }
    private State StockReserved { get; set; }
    private State StockReservationFailed { get; set; }
    private State PaymentCompleted { get; set; }
    private State PaymentFailed { get; set; }

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        _logger = logger;

        InstanceState(x => x.CurrentState);

        Event(() => CreateOrderMessage, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));
        Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => StockReservationFailedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Initially(
            When(CreateOrderMessage)
                .Then(context => { _logger.LogInformation($"CreateOrderMessage received in OrderStateMachine: {context.Saga} "); })
                .Then(context =>
                {
                    context.Saga.CustomerId = context.Message.CustomerId;
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                    context.Saga.PaymentAccountId = context.Message.PaymentAccountId;
                    context.Saga.TotalPrice = context.Message.TotalPrice;
                })
                .Publish(
                    context => new OrderCreatedEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        OrderItemList = context.Message.OrderItemList
                    })
                .TransitionTo(OrderCreated)
                .Then(context => { _logger.LogInformation($"OrderCreatedEvent published in OrderStateMachine: {context.Saga} "); }));

        During(OrderCreated,
            When(StockReservedEvent)
                .Then(context => { _logger.LogInformation($"StockReservedEvent received in OrderStateMachine: {context.Saga} "); })
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{QueuesConsts.CompletePaymentMessageQueueName}"),
                    context => new CompletePaymentMessage 
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        TotalPrice = context.Saga.TotalPrice,
                        CustomerId = context.Saga.CustomerId,
                        OrderItemList = context.Message.OrderItemList
                    })
                .Then(context => { _logger.LogInformation($"CompletePaymentMessage sent in OrderStateMachine: {context.Saga} "); }),
            When(StockReservationFailedEvent)
                .Then(context => { _logger.LogInformation($"StockReservationFailedEvent received in OrderStateMachine: {context.Saga} "); })
                .TransitionTo(StockReservationFailed)
                .Publish(
                    context => new OrderFailedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        CustomerId = context.Saga.CustomerId,
                        ErrorMessage = context.Message.ErrorMessage
                    })
                .Then(context => { _logger.LogInformation($"OrderFailedEvent published in OrderStateMachine: {context.Saga} "); })
        );

        During(StockReserved,
            When(PaymentCompletedEvent)
                .Then(context => { _logger.LogInformation($"PaymentCompletedEvent received in OrderStateMachine: {context.Saga} "); })
                .TransitionTo(PaymentCompleted)
                .Publish(
                    context => new OrderCompletedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        CustomerId = context.Saga.CustomerId
                    })
                .Then(context => { _logger.LogInformation($"OrderCompletedEvent published in OrderStateMachine: {context.Saga} "); })
                .Finalize(),
            When(PaymentFailedEvent)
                .Then(context => { _logger.LogInformation($"PaymentFailedEvent received in OrderStateMachine: {context.Saga} "); })
                .Publish(context => new OrderFailedEvent
                {
                    OrderId = context.Saga.OrderId,
                    CustomerId = context.Saga.CustomerId,
                    ErrorMessage = context.Message.ErrorMessage
                })
                .Then(context => { _logger.LogInformation($"OrderFailedEvent published in OrderStateMachine: {context.Saga} "); })
                .Send(new Uri($"queue:{QueuesConsts.StockRollBackMessageQueueName}"),
                    context => new StockRollbackMessage
                    {
                        OrderItemList = context.Message.OrderItemList
                    })
                .Then(context => { _logger.LogInformation($"StockRollbackMessage sent in OrderStateMachine: {context.Saga} "); })
                .TransitionTo(PaymentFailed)
        );
    }
}