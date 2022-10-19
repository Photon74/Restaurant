namespace Restaurant.Notification
{
    internal class Notifier
    {
        public void SendAsync(string message)
        {

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine(message);
            });
        }
    }
}
