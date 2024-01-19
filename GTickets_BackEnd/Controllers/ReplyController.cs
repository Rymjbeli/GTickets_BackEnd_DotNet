using Microsoft.AspNetCore.Mvc;
using GTickets_BackEnd.Models;
using GTickets_BackEnd.Repositories;
using GTickets_BackEnd.Services.Services;
using GTickets_BackEnd.Services.ServicesContracts;


namespace GTickets_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {

        public readonly IRepository<Reply, int> _repository;
        public readonly IReplyService _service;

        public ReplyController(IRepository<Reply, int> repository, IReplyService service)
        {
            _repository = repository;
            _service = service;
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
        public void Add(Reply reply)
        {
            _repository.Add(reply);
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