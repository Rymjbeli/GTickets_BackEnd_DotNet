using Microsoft.AspNetCore.Identity;

namespace GTickets_BackEnd.Models
{
    public class CustomUser : IdentityUser
    {
        // image path
        public string? Path { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<Reply>? Replies { get; set; }
    }
}
