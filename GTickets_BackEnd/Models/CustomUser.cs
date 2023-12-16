using Microsoft.AspNetCore.Identity;

namespace GTickets_BackEnd.Models
{
    public class CustomUser : IdentityUser
    {
        public string? Path { get; set; }
    }
}
