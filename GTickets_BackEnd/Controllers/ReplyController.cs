using Microsoft.AspNetCore.Mvc;
using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.Services;
using GTickets_BackEnd.Services.ServicesContracts;
using Microsoft.AspNetCore.Identity;


namespace GTickets_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {

        public readonly IRepository<Reply, int> _repository;
        public readonly IReplyService _service;
        private readonly IAuthService _authService;
        private readonly UserManager<CustomUser> _userManager;


        public ReplyController(IRepository<Reply, int> repository, IReplyService service, IAuthService authService, UserManager<CustomUser> userManager)
        {
            _repository = repository;
            _service = service;
            _authService = authService;
            _userManager = userManager;
        }

        // get all replies
        [HttpGet(Name = "GetAllReplies")]
        public ICollection<Reply> GetAll()
        {
            return _repository.GetAll();
        }

        //get reply by ticketId
        [HttpGet("ticketId/{ticketId}", Name = "GetAllRepliesByTicketId")]
        public ICollection<Reply> GetAllRepliesByTicketId(int ticketId)
        {
            return _service.GetAllRepliesByTicketId(ticketId);
        }

        //get + by id
        //route api/ticket/id/{id}
        [HttpGet("id/{id}", Name = "GetReplyById")]
        public Reply GetById(int id)
        {
            return _repository.GetById(id);
        }

        // add reply and fill all fields
        [HttpPost("add", Name = "AddReply")]
        public async Task<IActionResult> Add(Reply reply)
        {
            _repository.Add(reply);

            try
            {
                var userReply = reply.User;
                var ticketReply = reply.Ticket;
                var content = $"User reply: {userReply.UserName}\nTicket Title: {ticketReply.Title}\nTicket Id: {ticketReply.Id}";
                bool isAdmin = await _userManager.IsInRoleAsync(userReply, "Admin");
                if (isAdmin)
                {
                    var userTicket = await _userManager.FindByIdAsync(ticketReply.UserId);
                    _authService.SendEmail(userTicket.Email, "New Reply Added", content, "newReply");
                }
                else
                {
                    var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                    foreach (var adminUser in adminUsers)
                    {
                        _authService.SendEmail(adminUser.Email, "New Reply Added", content, "newReply");
                    }
                  
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // update reply by id
        [HttpPut("update/{id}/{content}", Name = "UpdateReply")]
        public void Update(int id, string content)
        {
            Reply reply = _repository.GetById(id);
            reply.Content = content;
            _repository.Update(reply);
        }

        // delete reply by id
        [HttpDelete("delete/{id}", Name = "DeleteReply")]
        public void Delete(int id)
        {
            try
            {
                Reply reply = _repository.GetById(id);
                _repository.Delete(reply);
            }
            catch
            {
                return;
            }

        }
    }
}