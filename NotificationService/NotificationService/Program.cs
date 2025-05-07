using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Infrastructures.Messaging;
using NotificationService.Infrastructures.Notifications;
using Serilog;
using Serilog.Events;

namespace NotificationService;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();

        try
        {
            Log.Information("[NotificationService] Starting up Notification Service...");

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.AddMassTransitConfiguration();
                    services.AddScoped<INotificationSender, MockNotificationSender>();
                })
                .Build();

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "[NotificationService] Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}