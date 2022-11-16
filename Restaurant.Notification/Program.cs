using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification.Consumers;

namespace Restaurant.Notification
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(mtc =>
                {
                    mtc.AddConsumer<NotifierTableBookedConsumer>();
                    mtc.AddConsumer<KitchenReadyConsumer>();

                    mtc.UsingRabbitMq((context, conf) =>
                    {
                        conf.UseMessageRetry(rc =>
                        {
                            rc.Exponential(5,
                                TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(100),
                                TimeSpan.FromSeconds(5));
                            rc.Ignore<StackOverflowException>();
                            rc.Ignore<ArgumentNullException>(e => e.Message.Contains("Consumer"));
                        });
                        conf.ConfigureEndpoints(context);
                    });
                });
                services.AddSingleton<Notifier>();
                //services.AddMassTransitHostedService(true);
            });
        }
    }
}