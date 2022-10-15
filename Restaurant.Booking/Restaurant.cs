using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Booking
{
    internal class Restaurant
    {
        private readonly List<Table> _tables = new();

        public Restaurant()
        {
            for (int i = 0; i < 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }
    }
}
