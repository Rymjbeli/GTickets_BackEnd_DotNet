using Microsoft.EntityFrameworkCore;

namespace GTickets_BackEnd.Models
{
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}


