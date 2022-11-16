using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking.Notifier;
using Restaurant.Messaging;

namespace Restaurant.Booking
{
    internal class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly INotifier _notifier;
        private readonly Restaurant _restaurant;

        public Worker(IBus bus, Restaurant restaurant, INotifier notifier)
        {
            _bus = bus;
            _restaurant = restaurant;
            _notifier = notifier;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
                await _notifier.SendMessageAsync("Здравствуйте! Желаете забронировать столик?", stoppingToken);

                var result = await _restaurant.BookFreeTableAsync(1, stoppingToken);

                await _bus.Publish(new TableBooked(NewId.NextGuid(), NewId.NextGuid(), result ?? false),
                    context => context.Durable = false, stoppingToken);
            }
        }
    }
}
