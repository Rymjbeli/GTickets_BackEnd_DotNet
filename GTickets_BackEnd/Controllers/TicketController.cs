using Microsoft.AspNetCore.Mvc;
using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.Services;
using GTickets_BackEnd.Services.ServicesContracts;


namespace GTickets_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        public readonly TicketRepository _repository;
        public readonly TicketService _service;

        public TicketController(IRepository<Ticket> repository, ITicketService service)
        {
            _repository = (TicketRepository)repository;
            _service = (TicketService?)service;
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
        [HttpPost("add/{userId}/{title}/{content}", Name = "AddTicket")]
        public void Add(string userId, string title, string content)
        {
            Ticket ticket = new Ticket();
            ticket.UserId = userId;
            ticket.Title = title;
            ticket.Content = content;
            _repository.Add(ticket);
        }

        // update ticket by id
        [HttpPut("update/{id}/{title}/{content}", Name = "UpdateTicket")]
        public void Update(int id, string title, string content)
        {
            Ticket ticket = _repository.GetById(id);
            ticket.Title = title;
            ticket.Content = content;
            _repository.Update(ticket);
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