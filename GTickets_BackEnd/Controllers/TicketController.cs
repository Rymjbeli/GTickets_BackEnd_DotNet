using Microsoft.AspNetCore.Mvc;
using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.Services;
using GTickets_BackEnd.Services.ServicesContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GTickets_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        public readonly TicketRepository _repository;
        public readonly ITicketService _service;
        private readonly ApplicationDBContext _context;
        private readonly IAuthService _authService;
        private readonly UserManager<CustomUser> _userManager;



        public TicketController(TicketRepository repository, ITicketService service, ApplicationDBContext context, IAuthService authService, UserManager<CustomUser> userManager)
        {
            _repository = repository;
            _service = service;
            _context = context;
            _authService = authService;
            _userManager = userManager;
        }

        // get all tickets
        [HttpGet(Name = "GetAllTickets")]
        public ICollection<Ticket> GetAll()
        {
            return _repository.GetAll();
        }

        //get ticket by userId
        [HttpGet("userId/{userId}", Name = "GetAllTicketsByUserId")]
        public ICollection<Ticket> GetAllTicketsByUserId(string userId)
        {
            return _service.GetAllTicketsByUserId(userId);
        }

        //get ticket by id
        //route api/ticket/id/{id}
        [HttpGet("id/{id}", Name = "GetTicketById")]
        public Ticket GetById(int id)
        {
            return _repository.GetById(id);
        }

        // add ticket and fill all fields
         [HttpPost("add", Name = "AddTicket")]
         public async Task<IActionResult> Add(Ticket ticket)
         {
             _repository.Add(ticket);

            try
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                foreach (var adminUser in adminUsers)
                {
                    _authService.SendEmail(adminUser.Email,"New Ticket Added", ticket.User.Email, "newTicket");
                }
                return Ok(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
            
            //var TicketCustomer = ticket.User?.UserName;
            ///var dashboardLink = "https://your-admin-dashboard-link";
            //var content = $"Ticket details: {TicketCustomer}\nDashboard Link: ";

        }




        // update ticket by id
        [HttpPut("update/{id}", Name = "UpdateTicket")]
        public IActionResult Update(int id, Ticket updatedTicket)
        {
            Ticket ticket = _repository.GetById(id);
            if (ticket == null)
            {
                return NotFound($"Ticket with ID {id} not found.");
            }
            ticket.Title = updatedTicket.Title;
            ticket.Content = updatedTicket.Content;
            ticket.Priority = updatedTicket.Priority;
            ticket.Status = updatedTicket.Status;
            ticket.UpdatedAt = updatedTicket.UpdatedAt;
            ticket.CreatedAt = updatedTicket.CreatedAt;
            _repository.Update(ticket);

            return Ok(ticket);
        }

        // delete ticket by id
        [HttpDelete("delete/{id}", Name = "DeleteTicket")]
        public void Delete(int id)
        {
            try
            {
                Ticket ticket = _repository.GetById(id);
                _repository.Delete(ticket);
            }
            catch
            {
                return;
            }

        }
    }
}