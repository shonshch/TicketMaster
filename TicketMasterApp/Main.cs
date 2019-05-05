using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TicketMasterApp
{
    public class main
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var events = new EvenGenrator();
                TaskAwaiter result = events.ReturnChapestTicket().GetAwaiter();
                result.GetResult();
                Thread.Sleep(TimeSpan.FromMinutes(30));
            }

        }
    }
}