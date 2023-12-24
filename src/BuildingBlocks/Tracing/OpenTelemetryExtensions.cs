using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Tracing;

public static class OpenTelemetryExtensions
{
    public static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetryParameters>(configuration.GetSection("OpenTelemetry"));
        var openTelemetryParameters = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryParameters>();

        ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(openTelemetryParameters.ActivitySourceName);

        services.AddOpenTelemetry().WithTracing(options =>
        {
            options.AddSource(openTelemetryParameters.ActivitySourceName)
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .ConfigureResource(resource =>
                {
                    resource.AddService(openTelemetryParameters.ServiceName, 
                        serviceVersion: openTelemetryParameters.ServiceVersion);
                });
            
            options.AddAspNetCoreInstrumentation(o =>
            {
                // to trace only api requests
                o.Filter = (context) => !string.IsNullOrEmpty(context.Request.Path.Value) && context.Request.Path.Value.Contains("Api", StringComparison.InvariantCulture);
                
                // example: only collect telemetry about HTTP GET requests
                // return httpContext.Request.Method.Equals("GET");
                
                // enrich activity with http request and response
                o.EnrichWithHttpRequest = (activity, httpRequest) =>
                {
                    activity.SetTag("requestProtocol", httpRequest.Protocol);
                };
                o.EnrichWithHttpResponse = (activity, httpResponse) =>
                {
                    activity.SetTag("responseLength", httpResponse.ContentLength);
                };
                
                // automatically sets Activity Status to Error if an unhandled exception is thrown
                o.RecordException = true;
                o.EnrichWithException = (activity, exception) =>
                {
                    activity.SetTag("exceptionType", exception.GetType().ToString());
                    activity.SetTag("stackTrace", exception.StackTrace);
                };
            });

            options.AddEntityFrameworkCoreInstrumentation(opt =>
            {
                opt.SetDbStatementForText = true;
                opt.SetDbStatementForStoredProcedure = true;
                opt.EnrichWithIDbCommand = (activity, command) =>
                {
                    // var stateDisplayName = $"{command.CommandType} main";
                    // activity.DisplayName = stateDisplayName;
                    // activity.SetTag("db.name", stateDisplayName);
                };
            });
            
            //options.AddConsoleExporter();
            
            options.AddOtlpExporter();
        });
        
    }
}