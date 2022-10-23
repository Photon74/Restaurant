using Restaurant.Notification;
using Restaurant.Messaging;

namespace Restaurant.Booking
{
    internal class Restaurant
    {
        private readonly Notifier _notifier = new();
        private readonly Produser _produser = new ("BookingNotification", "localhost");
        private readonly List<Table> _tables = new();
        private readonly int delay = 5000;

        public Restaurant()
        {
            for (int i = 1; i < 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTable(int countOfPersons)
        {
            _notifier.SendAsync("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь, оставайтесь на линии.");

            var table = _tables.FirstOrDefault(t =>
                                                    t.SeatsCount >= countOfPersons
                                                    && t.State == State.Free);
            Thread.Sleep(delay);
            table?.SetState(State.Booked);

            _notifier.SendAsync(table is null
                ? "К сожалению все столики заняты."
                : $"Готово! Ваш столик номер {table.Id}");
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            _notifier.SendAsync("Подождите секунду, я подберу столик и подтвержу вашу бронь, Вам придет уведомление.");

            Task.Run(async () =>
            {
                Table? table;
                lock (_tables)
                {
                    table = _tables.FirstOrDefault(t =>
                                                   t.SeatsCount >= countOfPersons
                                                   && t.State == State.Free);
                    table?.SetState(State.Booked);
                }

                _produser.Send(table is null
                ? "УВЕДОМЛЕНИЕ: К сожалению все столики заняты."
                : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
            });
        }

        public void UnBookTableAsync(int tableId)
        {
            
            Task.Run(async () =>
            {
                Table? table;
                lock (_tables)
                {
                    table = _tables.FirstOrDefault(t => t.Id == tableId && t.State == State.Booked);
                    table?.SetState(State.Free); 
                }

                _notifier.SendAsync(table is null
                    ? $"Столик под номером {tableId} не занят!"
                    : $"Снята бронь со столика номер {tableId}.");
            });
        }

        public void UnBookTable(int tableId)
        {
            var table = _tables.FirstOrDefault(t => t.Id == tableId && t.State == State.Booked);
            Thread.Sleep(delay);
            table?.SetState(State.Free);

            _notifier.SendAsync(table is null
                ? $"Столик под номером {tableId} не занят!"
                : $"Снята бронь со столика номер {tableId}.");
        }

        public void AutoUnbookTables(object obj)
        {
            Task.Run(() =>
            {
                var tables = _tables.Where(t => t.State == State.Booked);
                if (!tables.Any()) _notifier.SendAsync("Все столики свободны!");
                foreach (var table in tables)
                {
                    if (table != null)
                    {
                        table.SetState(State.Free);
                        _notifier.SendAsync($"Снята бронь со столика {table.Id}");
                    }
                }
            });
        }
    }
}
