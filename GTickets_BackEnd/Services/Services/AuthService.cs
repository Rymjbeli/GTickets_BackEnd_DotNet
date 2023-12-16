using GTickets_BackEnd.Services.ServicesContracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Management.Service.Models;
using User.Management.Service.Services;

namespace GTickets_BackEnd.Services.Services
{
    public class AuthService : IAuthService 
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public AuthService(
            IConfiguration configuration,
            IEmailService emailService
            ) { 
            _configuration = configuration;
            _emailService = emailService;
        }
        public string generateUniqueFilename()
        {
            // Use timestamp or any other logic to generate a unique identifier
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddDays(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            return token;

        }

        public void SendEmail(string email, string msg, string toSend, string emailType)
        {
            var message = new Message(
                new string[] { email! },
                msg,
                toSend!
                );

            _emailService.SendEmail(message, emailType);

        }
    }
}
