using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.ServicesContracts;

namespace GTickets_BackEnd.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = (UserRepository)userRepository;
        }

        public bool IsEmailConfirmed(string id)
        {
            var user = _userRepository.GetById(id);
            return user.EmailConfirmed;
        }
    }
}