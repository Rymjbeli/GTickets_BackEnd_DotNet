using System.ComponentModel.DataAnnotations;

namespace GTickets_BackEnd.Models.Authentication.Login
{
    public class LoginModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }
    }
}
