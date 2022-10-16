using Restaurant.Notification;
using System.Diagnostics;
using System.Threading;

namespace Restaurant.Booking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var restaurant = new Restaurant();
            var tm = new TimerCallback(restaurant.AutoUnbookTables);
            var timer = new Timer(tm, 0, 0, 20000);
            while (true)
            {
                Console.WriteLine("Привет! Желаете забронировать столик?\n1 - мы уведомим Вас по смс (асинхронно)" +
                    "\n2 - подождите на линии, мы Вас оповестим (синхронно)");

                if (!int.TryParse(Console.ReadLine(), out var choice) || choice is not (1 or 2))
                {
                    Console.WriteLine("Введите, пожалуйста 1 или 2");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                if (choice == 1)
                    restaurant.BookFreeTableAsync(1); 
                else
                    restaurant.BookFreeTable(1);
                Console.WriteLine("Спасибо за Ваше обращение!");

                stopWatch.Stop();
                var time = stopWatch.Elapsed;
                Console.WriteLine($"Время обработки заказа - {time.Seconds:00}:{time.Milliseconds:00}");
            }
        }
    }
}