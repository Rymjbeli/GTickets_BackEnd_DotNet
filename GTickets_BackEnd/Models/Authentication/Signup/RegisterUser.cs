using System.ComponentModel.DataAnnotations;

namespace GTickets_BackEnd.Models.Authentication.Signup
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="UserName is required")]
        public string? Username { get; set; }
        public IFormFile? File { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }
    }
}
