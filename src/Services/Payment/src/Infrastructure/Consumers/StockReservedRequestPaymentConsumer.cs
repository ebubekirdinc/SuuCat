using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;
using Shared.Interfaces;

namespace Payment.Infrastructure.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPayment>
    {
        private readonly ILogger<StockReservedRequestPaymentConsumer> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedRequestPaymentConsumer(ILogger<StockReservedRequestPaymentConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestPayment> context)
        {
            var balance = 3000m;

            if (balance > context.Message.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.TotalPrice} was withdrawn from credit card for user id= {context.Message.CustomerId}");

                await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation($"{context.Message.TotalPrice} was not withdrawn from credit card for user id={context.Message.CustomerId}");
            
                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId) { Reason = "not enough balance", OrderItems = context.Message.OrderItems });
            }
        }
    }
}