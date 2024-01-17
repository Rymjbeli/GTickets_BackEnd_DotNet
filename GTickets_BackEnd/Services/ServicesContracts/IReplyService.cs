using GTickets_BackEnd.Models;

namespace GTickets_BackEnd.Services.ServicesContracts
{
    public interface IReplyService
    {
        public ICollection<Reply> GetAllRepliesByTicketId(int ticketId);
    }
}
