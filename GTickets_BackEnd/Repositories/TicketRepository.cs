using GTickets_BackEnd.Models;

namespace GTickets_BackEnd.Repositories
{
    public class TicketRepository : IRepository<Ticket>
    {
        public readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Ticket> GetAll()
        {
            return _context.Tickets.ToList();
        }

        public Ticket GetById(int id)
        {
            return _context.Tickets.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Ticket entity)
        {
            _context.Tickets.Add(entity);
            _context.SaveChanges();
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

        public ICollection<Ticket> GetAllTicketsByUserId(int userId)
        {
            return _context.Tickets.Where(t => t.UserId == userId).ToList();
        }

    }

}
