namespace Restaurant.Booking.Notifier
{
    public interface INotifier
    {
        Task SendMessageAsync(string message, CancellationToken cancellationToken);
    }
}