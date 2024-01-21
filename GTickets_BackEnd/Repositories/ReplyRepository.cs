using Microsoft.AspNetCore.Mvc;
using GTickets_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace GTickets_BackEnd.Repositories
{
    public class ReplyRepository : IRepository<Reply, int>
    {
        private readonly ApplicationDBContext _context;

        public ReplyRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public ICollection<Reply> GetAll()
        {
            return _context
                .Replies
                .Include(c => c.Ticket)
                .Include(c => c.User)
                .ToList();
        }

        public Reply GetById(int id)
        {
            return _context
                .Replies
                .Include(c => c.Ticket)
                .Include(c => c.User)
                .FirstOrDefault(r => r.Id == id)!;
        }

        public Reply Add(Reply entity)
        {
            _context.Replies.Add(entity);
            _context.SaveChanges();
            return _context.Replies.Include(t => t.User).Include(t => t.Ticket).FirstOrDefault(t => t.Id == entity.Id);

        }

        public void Update(Reply entity)
        {
            _context.Replies.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Reply entity)
        {
            _context.Replies.Remove(entity);
            _context.SaveChanges();
        }

        public ICollection<Reply> GetAllRepliesByTicketId(int ticketId)
        {
            return _context
                .Replies
                .Where(t => t.TicketId == ticketId)
                .Include(c => c.Ticket)
                .Include(c => c.User)
                .ToList();
        }
    }
}
