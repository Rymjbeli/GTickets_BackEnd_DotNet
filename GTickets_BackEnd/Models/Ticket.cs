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
        public int? UserId { get; set; }
        public CustomUser? User { get; set; }
        public ICollection<Reply>? Replies { get; set; }

    }
}
