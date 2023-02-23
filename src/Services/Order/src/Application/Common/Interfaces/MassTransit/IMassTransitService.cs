namespace Order.Application.Common.Interfaces.MassTransit;

public interface IMassTransitService
{
    Task Send<T>(T message, string queueName) where T : class;
}