using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GTickets_BackEnd.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Priority { get; set; }
        public string? UserId { get; set; }
        public CustomUser? User { get; set; }
        [JsonIgnore]
        public ICollection<Reply>? Replies { get; set; }

    }
}
