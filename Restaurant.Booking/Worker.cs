using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking.Notifier;
using Restaurant.Messaging;

namespace Restaurant.Booking
{
    /// <summary>
    /// Сервис бронирования столика
    /// </summary>
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

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(10000, cancellationToken);
                await _notifier.SendMessageAsync("Здравствуйте! Желаете забронировать столик?", cancellationToken);

                var result = await _restaurant.BookFreeTableAsync(1, cancellationToken);

                await _bus.Publish(new TableBooked(NewId.NextGuid(), NewId.NextGuid(), result ?? false),
                    context => context.Durable = false, cancellationToken);
            }
        }
    }
}
