using System.Diagnostics;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TacticWar.Rest.Installers;

public static class OpenTelemetryInstaller
{
    public static ActivitySource ActivitySource { get; } = new("TacticWarActivitySource");

    public static void AddAppOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry().WithTracing(builder =>
                {
                    builder.AddAspNetCoreInstrumentation()
                           .AddSource(ActivitySource.Name)
                           .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("TacticWar"))
                           .AddConsoleExporter()
                           .AddOtlpExporter();
                })
                .UseAzureMonitorIfEnabled(configuration);
    }

    static OpenTelemetryBuilder UseAzureMonitorIfEnabled(this OpenTelemetryBuilder openTelemetryBuilder, IConfiguration configuration)
    {
        var azureMonitorCanBeEnabled = !string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
        return azureMonitorCanBeEnabled ? openTelemetryBuilder.UseAzureMonitor() : openTelemetryBuilder;
    }
}
