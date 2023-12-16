using System.ComponentModel.DataAnnotations;

namespace GTickets_BackEnd.Models.Authentication.Signup
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
