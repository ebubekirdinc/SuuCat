namespace Shared.Constants
{
    public class QueuesConsts
    {
        // events
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string OrderRequestCompletedEventtQueueName = "order-request-completed-queue";
        public const string OrderRequestFailedEventtQueueName = "order-request-failed-queue";
        
        // messages
        public const string CreateOrderMessageQueueName = "create-order-message-queue";
        public const string CompletePaymentMessageQueueName = "complete-payment-message-queue";
        public const string StockRollBackMessageQueueName = "stock-rollback-message-queue";
    }
}