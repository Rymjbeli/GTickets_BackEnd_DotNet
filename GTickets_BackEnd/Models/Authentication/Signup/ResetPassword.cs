using System.ComponentModel.DataAnnotations;

namespace GTickets_BackEnd.Models.Authentication.Signup
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;

    }
}
