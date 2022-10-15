using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Notification
{
    internal class Notifier
    {
        public void SendAsync(string message)
        {

            Task.Run(async () => 
            {
                await Task.Delay(10000);
                Console.WriteLine(message);
            });
        }
    }
}
