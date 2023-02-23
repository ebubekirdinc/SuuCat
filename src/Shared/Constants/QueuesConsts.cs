namespace Shared.Constants
{
    public class QueuesConsts
    {
        public const string OrderCreatedRequest = "order-created-request-queue";
        public const string PaymentStockReservedRequestQueueName = "payment-stock-reserved-request-queue";
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockRollBackMessageQueueName = "stock-rollback-queue";
        public const string OrderRequestCompletedEventtQueueName = "order-request-completed-queue";
        public const string OrderRequestFailedEventtQueueName = "order-request-failed-queue";
    }
}