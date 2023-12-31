using System.Diagnostics.Metrics;

namespace Tracing;

public static class OpenTelemetryMetric
{
    // Identity metrics
    private static readonly Meter IdentityMeter = new("Identity.Api");
    public static readonly Counter<int> UserCreatedEventCounter = IdentityMeter.CreateCounter<int>("user.created.event.count");
    
    // Order metrics
    private static readonly Meter OrderMeter = new("Order.Api");
    public static readonly Counter<int> OrderStartedEventCounter = OrderMeter.CreateCounter<int>("order.started.event.count");
    public static readonly Counter<int> OrderLongRunningRequestCounter = OrderMeter.CreateCounter<int>("order.long.running.request.count");
    public static readonly Histogram<long> OrderMethodDuration = OrderMeter.CreateHistogram<long>("order.method.duration", "milliseconds");
    
    // Subscription metrics
    private static readonly Meter StockMeter = new("Subscription.Api");
    public static readonly UpDownCounter<int> StockCounter = StockMeter.CreateUpDownCounter<int>("subscription.stock.count");
}