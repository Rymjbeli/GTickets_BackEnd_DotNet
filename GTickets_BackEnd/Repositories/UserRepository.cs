using GTickets_BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GTickets_BackEnd.Repositories

{
    public class UserRepository
    {
        private readonly UserManager<CustomUser> _userManager;

        public UserRepository(UserManager<CustomUser> userManager)
        {
            _userManager = userManager;
        }

        public ICollection<CustomUser> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public CustomUser GetById(string id)
        {
            return _userManager.Users.FirstOrDefault(u => u.Id == id);
        }

    }
}
