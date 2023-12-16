using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GTickets_BackEnd.Services.ServicesContracts
{
    public interface IAuthService
    {
        public string generateUniqueFilename();
        public JwtSecurityToken GetToken(List<Claim> authClaims);
        public void SendEmail(string email, string msg, string toSend, string emailType);
    }

}
