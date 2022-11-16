namespace Restaurant.Booking.Notifier
{
    public class Notifier : INotifier
    {
        public Task SendMessageAsync(string message, CancellationToken cancellationToken) =>
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine(message);
            });
    }
}
