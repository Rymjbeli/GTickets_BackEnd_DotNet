
using GTickets_BackEnd.Models;

namespace GTickets_BackEnd.Services.ServicesContracts
{
    public interface ITicketService
    {
        public ICollection<Ticket> GetAllTicketsByUserId(int userId);
    }
}
