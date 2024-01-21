using GTickets_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace GTickets_BackEnd.Repositories
{
    public class TicketRepository
    {
        public readonly ApplicationDBContext _context;

        public TicketRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public ICollection<Ticket> GetAll()
        {
            return _context.Tickets.Include(t => t.User).ToList();
        }

        public Ticket GetById(int id)
        {
            return _context.Tickets.Include(t => t.User).FirstOrDefault(t => t.Id == id);
        }

        public Ticket Add(Ticket entity)
        {
            _context.Tickets.Add(entity);
            _context.SaveChanges();
            return _context.Tickets.Include(t => t.User).FirstOrDefault(t => t.Id == entity.Id);
        }

        public void Update(Ticket entity)
        {
            _context.Tickets.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Ticket entity)
        {
            _context.Tickets.Remove(entity);
            _context.SaveChanges();
        }

        public ICollection<Ticket> GetAllTicketsByUserId(string userId)
        {
            return _context.Tickets.Where(t => t.UserId == userId).ToList();
        }

    }

}
