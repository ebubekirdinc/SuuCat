using MassTransit;
using Order.Application.Common.Interfaces.MassTransit;

namespace Order.Infrastructure.MassTransit;

public class MassTransitService : IMassTransitService
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MassTransitService(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Send<T>(T message, string queueName) where T : class
    {
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
        
        await sendEndpoint.Send<T>(message);
    }
}