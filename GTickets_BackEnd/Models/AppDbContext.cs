using Microsoft.EntityFrameworkCore;

namespace GTickets_BackEnd.Models
{
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Reply> Replies { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .IsRequired();

            builder.Entity<Reply>()
            .HasOne(t => t.User)
            .WithMany(u => u.Replies)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
        }
    }
}


