using EventBus.Constants;
using EventBus.Events;
using EventBus.Events.Interfaces;
using EventBus.Messages;
using EventBus.Messages.Interfaces;
using MassTransit;
using SagaOrchestrationStateMachine.StateInstances;
using ILogger = Serilog.ILogger;

namespace SagaOrchestrationStateMachine.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    private readonly ILogger _logger;

    // Commands
    private Event<ICreateOrderMessage> CreateOrderMessage { get; set; }

    // Events
    public Event<IStockReservedEvent> StockReservedEvent { get; set; }
    public Event<IStockReservationFailedEvent> StockReservationFailedEvent { get; set; }
    public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }

    // States
    public State OrderCreated { get; set; }
    public State StockReserved { get; set; }
    public State StockReservationFailed { get; set; }
    public State PaymentCompleted { get; set; }
    public State PaymentFailed { get; set; }

    public OrderStateMachine() //ILogger<OrderStateMachine> logger)
    {
        _logger = Serilog.Log.Logger; 
        InstanceState(x => x.CurrentState);

        Event(() => CreateOrderMessage, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));
        Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => StockReservationFailedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Initially(
            When(CreateOrderMessage)
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("CreateOrderMessage received in OrderStateMachine: {ContextSaga} ", context.Saga); })
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
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("OrderCreatedEvent published in OrderStateMachine: {ContextSaga} ", context.Saga); }));

        During(OrderCreated,
            When(StockReservedEvent)
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("StockReservedEvent received in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{QueuesConsts.CompletePaymentMessageQueueName}"),
                    context => new CompletePaymentMessage 
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        TotalPrice = context.Saga.TotalPrice,
                        CustomerId = context.Saga.CustomerId,
                        OrderItemList = context.Message.OrderItemList
                    })
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("CompletePaymentMessage sent in OrderStateMachine: {ContextSaga} ", context.Saga); }),
            When(StockReservationFailedEvent)
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("StockReservationFailedEvent received in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .TransitionTo(StockReservationFailed)
                .Publish(
                    context => new OrderFailedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        CustomerId = context.Saga.CustomerId,
                        ErrorMessage = context.Message.ErrorMessage
                    })
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("OrderFailedEvent published in OrderStateMachine: {ContextSaga} ", context.Saga); })
        );

        During(StockReserved,
            When(PaymentCompletedEvent)
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("PaymentCompletedEvent received in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .TransitionTo(PaymentCompleted)
                .Publish(
                    context => new OrderCompletedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        CustomerId = context.Saga.CustomerId
                    })
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("OrderCompletedEvent published in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .Finalize(),
            When(PaymentFailedEvent)
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("PaymentFailedEvent received in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .Publish(context => new OrderFailedEvent
                {
                    OrderId = context.Saga.OrderId,
                    CustomerId = context.Saga.CustomerId,
                    ErrorMessage = context.Message.ErrorMessage
                })
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("OrderFailedEvent published in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .Send(new Uri($"queue:{QueuesConsts.StockRollBackMessageQueueName}"),
                    context => new StockRollbackMessage
                    {
                        OrderItemList = context.Message.OrderItemList
                    })
                .Then(context => { _logger.ForContext("CorrelationId", context.Saga.CorrelationId).Information("StockRollbackMessage sent in OrderStateMachine: {ContextSaga} ", context.Saga); })
                .TransitionTo(PaymentFailed)
        );
    }
}