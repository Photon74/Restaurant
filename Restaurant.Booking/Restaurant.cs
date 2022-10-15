using Restaurant.Notification;

namespace Restaurant.Booking
{
    internal class Restaurant
    {
        private readonly Notifier _notifier = new();
        private readonly List<Table> _tables = new();
        private readonly int delay = 5000;

        public Restaurant()
        {
            for (int i = 0; i < 10; i++)
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
            _notifier.SendAsync("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь, Вам придет уведомление.");

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t =>
                                                    t.SeatsCount >= countOfPersons
                                                    && t.State == State.Free);
                await Task.Delay(delay);
                table?.SetState(State.Booked);

                _notifier.SendAsync(table is null
                ? "УВЕДОМЛЕНИЕ: К сожалению все столики заняты."
                : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
            });
        }

        public void UnBookTableAsync(int tableId)
        {
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == tableId && t.State == State.Booked);
                await Task.Delay(delay);
                table?.SetState(State.Free);

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
    }
}
