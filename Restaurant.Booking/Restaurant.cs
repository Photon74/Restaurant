using Restaurant.Booking.Notifier;

namespace Restaurant.Booking
{
    internal class Restaurant
    {
        private readonly INotifier _notifier;
        private readonly List<Table> _tables = new();
        private readonly int delay = 5000;

        public Restaurant(INotifier notifier)
        {
            _notifier = notifier;

            for (int i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        /// <summary>
        /// Попытка брони столика (Async)
        /// </summary>
        /// <param name="countOfPersons">Кол-во гостей</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool?> BookFreeTableAsync(int countOfPersons, CancellationToken cancellationToken)
        {
            await _notifier.SendMessageAsync("Подождите секунду, я подберу столик и подтвержу вашу бронь, Вам придет уведомление.", cancellationToken);

            var table = _tables.FirstOrDefault(t =>
                                      t.SeatsCount >= countOfPersons
                                      && t.State == State.Free);

            return table?.SetState(State.Booked);
        }

        /// <summary>
        /// Снятие брони столика по его Id
        /// </summary>
        /// <param name="tableId">Id столика</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool?> UnBookTableAsync(int tableId, CancellationToken cancellationToken)
        {
            var table = _tables.FirstOrDefault(t => t.Id == tableId && t.State == State.Booked);

            await _notifier.SendMessageAsync(table is null
                        ? $"Столик под номером {tableId} не занят!"
                        : $"Снята бронь со столика номер {tableId}.", cancellationToken);

            return table?.SetState(State.Free);
        }

        /// <summary>
        /// Автоматическое снятие брони столика
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        public void AutoUnbookTables(object obj, CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                var tables = _tables.Where(t => t.State == State.Booked);
                if (!tables.Any()) _notifier.SendMessageAsync("Все столики свободны!", cancellationToken);
                foreach (var table in tables)
                {
                    if (table != null)
                    {
                        table.SetState(State.Free);
                        _notifier.SendMessageAsync($"Снята бронь со столика {table.Id}", cancellationToken);
                    }
                }
            }, cancellationToken);
        }
    }
}
