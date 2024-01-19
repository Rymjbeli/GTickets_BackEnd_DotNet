using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.ServicesContracts;

namespace GTickets_BackEnd.Services.Services
{
    public class TicketService : ITicketService
    {

        // ticketRepository
        private readonly TicketRepository _repository;

        public TicketService(TicketRepository repository)
        {
            _repository = repository;
        }

        // get all tickets by userId
        public ICollection<Ticket> GetAllTicketsByUserId(string userId)
        {
            return _repository.GetAllTicketsByUserId(userId);
        }
    }
}