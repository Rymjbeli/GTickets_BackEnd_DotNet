using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GTickets_BackEnd.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? TicketId { get; set; }
        
        public Ticket? Ticket { get; set; }
        public string? UserId { get; set; }
        public CustomUser? User { get; set; }
    }
}

