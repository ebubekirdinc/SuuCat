using EventBus.Events;
using EventBus.Messages.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Infrastructure.Consumers;

public class CompletePaymentMessageConsumer : IConsumer<ICompletePaymentMessage>
{
    private readonly ILogger<CompletePaymentMessageConsumer> _logger;

    private readonly IPublishEndpoint _publishEndpoint;

    public CompletePaymentMessageConsumer(ILogger<CompletePaymentMessageConsumer> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<ICompletePaymentMessage> context)
    {
        // todo payment from stripe service
        var paymentSuccess = DateTime.UtcNow.Second % 2 == 0;

        // if (paymentSuccess)
        // {
            _logger.LogInformation("Payment successfull. {MessageTotalPrice}$ was withdrawn from user with Id= {MessageCustomerId} and correlation Id={MessageCorrelationId}",
                context.Message.TotalPrice, context.Message.CustomerId, context.Message.CorrelationId);

            await _publishEndpoint.Publish(new PaymentCompletedEvent
            {
                CorrelationId = context.Message.CorrelationId
            });

            return;
        // }
        //
        // _logger.LogInformation("Payment failed. {MessageTotalPrice}$ was not withdrawn from user with Id={MessageCustomerId} and correlation Id={MessageCorrelationId}",
        //     context.Message.TotalPrice, context.Message.CustomerId, context.Message.CorrelationId);
        //
        // await _publishEndpoint.Publish(new PaymentFailedEvent
        // {
        //     CorrelationId = context.Message.CorrelationId,
        //     ErrorMessage = "Payment failed", 
        //     OrderItemList = context.Message.OrderItemList
        // });
    }
}