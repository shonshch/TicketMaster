using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TicketMasterApp
{
    public class main
    {
        static void Main(string[] args)
        {
            var events = new EvenGenrator();
            TaskAwaiter result = events.ReturnChapestTicket().GetAwaiter();
            result.GetResult();
        }
    }
}