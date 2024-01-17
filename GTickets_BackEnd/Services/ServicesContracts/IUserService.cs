using GTickets_BackEnd.Models;

namespace GTickets_BackEnd.Services.ServicesContracts
{
    public interface IUserService
    {
        public Boolean IsEmailConfirmed(string userId);
    }
}