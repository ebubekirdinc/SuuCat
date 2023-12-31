namespace EventBus.Constants;

public class QueuesConsts
{
    // events
    public const string OrderCreatedEventQueueName = "order-created-queue";
    public const string OrderCompletedEventQueueName = "order-completed-queue";
    public const string OrderFailedEventQueueName = "order-failed-queue";
    public const string UserCreatedEventQueueName = "user-created-event-queue";

    // messages
    public const string CreateOrderMessageQueueName = "create-order-message-queue";
    public const string CompletePaymentMessageQueueName = "complete-payment-message-queue";
    public const string StockRollBackMessageQueueName = "stock-rollback-message-queue";
}