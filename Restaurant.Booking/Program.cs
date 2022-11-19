using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking.Notifier;
//using Restaurant.Notification.Consumers;

namespace Restaurant.Booking
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var builder = WebApplication.CreateBuilder(args);
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddMassTransit(configure =>
                {
                    configure.UsingRabbitMq((context, conf) =>
                    {
                        conf.ConfigureEndpoints(context);
                    });
                });
                services.AddTransient<Restaurant>();
                services.AddHostedService<Worker>();
                services.AddScoped<INotifier, Notifier.Notifier>();
            });
    }
}