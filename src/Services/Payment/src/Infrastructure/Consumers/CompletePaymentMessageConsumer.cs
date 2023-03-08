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