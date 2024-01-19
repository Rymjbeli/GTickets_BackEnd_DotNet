using GTickets_BackEnd.Services.ServicesContracts;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Models;

namespace GTickets_BackEnd.Services.Services
{
    public class ReplyService : IReplyService
    {
        private readonly ReplyRepository _repository;

        public ReplyService(IRepository<Reply, int> repository)
        {
              _repository = (ReplyRepository)repository;
        }

        public ICollection<Reply> GetAllRepliesByTicketId(int ticketId)
        {
            return _repository.GetAllRepliesByTicketId(ticketId);
            //return _repository.GetAll().Where(r => r.TicketId == ticketId).ToList();
        }
    }
}
