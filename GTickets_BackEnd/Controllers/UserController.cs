using Microsoft.AspNetCore.Mvc;
using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.Services;
using GTickets_BackEnd.Services.ServicesContracts;

namespace GTickets_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly UserRepository _userRepository;
        public readonly IUserService _userService;

        public UserController(UserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet("all", Name = "GetAllUsers")]
        public ICollection<CustomUser> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public CustomUser GetUserById(string id)
        {
            return _userRepository.GetById(id);
        }

        [HttpGet("isVerified/{id}", Name = "IsEmailConfirmed")]
        public bool isVerified(string id)
        {
            return _userService.IsEmailConfirmed(id);
        }
    }
}
