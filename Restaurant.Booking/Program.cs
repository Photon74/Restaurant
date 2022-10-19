using System.Diagnostics;

namespace Restaurant.Booking
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var restaurant = new Restaurant();
            var timerCallback = new TimerCallback(restaurant.AutoUnbookTables);
            var timer = new Timer(timerCallback, 0, 0, 20000);

            while (true)
            {
                await Task.Delay(2000);

                Console.WriteLine("Привет! Желаете забронировать столик?");

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                restaurant.BookFreeTableAsync(1);
                Console.WriteLine("Спасибо за Ваше обращение!");

                stopWatch.Stop();
                var time = stopWatch.Elapsed;
                Console.WriteLine($"Время обработки заказа - {time.Seconds:00}:{time.Milliseconds:00}");
            }
        }
    }
}