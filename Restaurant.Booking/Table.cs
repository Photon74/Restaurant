namespace Restaurant.Booking
{
    internal class Table
    {
        public int Id { get; set; }

        public State State { get; private set; }

        public int SeatsCount { get; }


        public Table(int id)
        {
            Id = id;
            State = State.Free;
            SeatsCount = random.Next(2, 5);
        }

        public bool SetState(State state)
        {
            lock (_lock)
            {
                if (state == State) return false;

                State = state;
                return true; 
            }
        }

        private readonly Random random = new();
        private readonly object _lock = new ();
    }
}
