using GTickets_BackEnd.Models;
using GTickets_BackEnd.Models.Authentication.Login;
using GTickets_BackEnd.Models.Authentication.Signup;
using GTickets_BackEnd.Services.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using User.Management.Service.Services;

namespace GTickets_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        public AuthenticationController(
            UserManager<CustomUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration,
            IEmailService emailService,
            IAuthService authService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _authService = authService;
        
        }

        //localhost:7027/api/Authentication/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUser registerUser)
        {
            if (registerUser == null) throw new ArgumentNullException(nameof(registerUser));

            // Additional validation for RegisterUser properties

            // Check if the user already exists
            var userExistByEmail = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExistByEmail != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User Already Exists! " });
            }
            var userExistByUsername = await _userManager.FindByNameAsync(registerUser.Username);
            if (userExistByUsername != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "This username already exists! Please choose another one. " });
            }

            // Add the user to the database
            CustomUser user = new CustomUser
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                // TwoFactorEnabled = true
            };

            // Handle file upload
            if (registerUser.File != null)
            {
                // Process the uploaded file here
                var originalFilename = registerUser.File.FileName;
                var fileExtension = Path.GetExtension(originalFilename);

                // Generate a unique filename using a timestamp or unique identifier
                var uniqueFilename = $"{_authService.generateUniqueFilename()}{fileExtension}";

                var uploadDir = Path.Combine("wwwroot", "uploads"); // assuming a folder named "uploads" in the wwwroot directory
                var filePath = Path.Combine(uploadDir, uniqueFilename);

                // Create the uploads directory if it doesn't exist
                Directory.CreateDirectory(uploadDir);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await registerUser.File.CopyToAsync(stream);
                }

                // Save the relative file path in the user entity
                user.Path = Path.Combine("uploads", uniqueFilename);
            }



            var result = await _userManager.CreateAsync(user, registerUser.password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "user Failed to create" });
            }
            // Add role to user
            await _userManager.AddToRoleAsync(user, "Admin");

            // Add Token to verify email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
                "Authentication",
                new { token, email = user.Email },
                Request.Scheme
                );
            _authService.SendEmail(user.Email, "Confirmation Email Link", confirmationLink, "verif");


            return StatusCode(StatusCodes.Status201Created,
                new Response
                {
                    Status = "Success",
                    Message = $"User Created & Email sent to {user.Email} Successfully!"
                });
        }

        


        [HttpPut("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] EmailRequest emailRequest)
        {
            var email = emailRequest.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "User not found." });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
                "Authentication",
                new { token, email = user.Email },
                Request.Scheme
            );

            _authService.SendEmail(user.Email, "Confirmation Email Link", confirmationLink, "verif");

            return StatusCode(StatusCodes.Status200OK,
                new Response { Status = "Success", Message = "Confirmation email link resent successfully." });
        }



        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync (email);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    var redirectUrl = "http://localhost:4200/verify-account";
                    return Redirect(redirectUrl);

                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "this User doesn't exist" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            //checking the user
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user != null & await _userManager.CheckPasswordAsync(user, loginModel.password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                /*if (user.TwoFactorEnabled)
                {
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    SendEmail(user.Email, "OTP confirmation ", token);

                    return StatusCode(StatusCodes.Status201Created,
                    new Response
                    {
                        Status = "Success",
                        Message = $"We have sent an OTP to your Email {user.Email}!"
                    });
                }*/
                
                var jwtToken = _authService.GetToken(authClaims);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo,
                    userId = user.Id,
                    role = userRoles.FirstOrDefault()
                });

            }
            return Unauthorized(); 
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]

        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if( user != null )
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(
                    nameof(ResetPassword), 
                    "Authentication", 
                    new {
                        token, 
                        email = user.Email  
                    }, 
                    Request.Scheme
                    );

                _authService.SendEmail(email, "Forgot Password Link", forgotPasswordLink, "reset");
                return StatusCode(StatusCodes.Status200OK,
                    new Response { 
                        Status = "Success", 
                        Message = $"Password changed request is sent on Email {user.Email}, Please open your email "
                    }  
                    );
                
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new Response
                    {
                        Status = "Error",
                        Message = $"Could not send link, please try again "
                    }
                    );
        }


        [HttpGet("reset-passsword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            var redirectUrl = $"http://localhost:4200/reset-password?email={HttpUtility.UrlEncode(email)}&token={HttpUtility.UrlEncode(token)}";

            return Redirect(redirectUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]

        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(
                    user, 
                    resetPassword.Token, 
                    resetPassword.Password
                    );
                if (!resetPassResult.Succeeded)
                {
                    foreach(var error  in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK,
                    new Response
                    {
                        Status = "Success",
                        Message = "Password has been changed "
                    }
                    );
                
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new Response
                    {
                        Status = "Error",
                        Message = $"Could not send link, please try again "
                    }
                    );
        }
    }
}
