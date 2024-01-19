using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace GTickets_BackEnd.Models
{
    public class CustomUser : IdentityUser
    {
        // image path
        public string? Path { get; set; }
        [JsonIgnore]
        public ICollection<Ticket>? Tickets { get; set; }
        [JsonIgnore]
        public ICollection<Reply>? Replies { get; set; }
    }
}
