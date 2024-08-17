using IMAP.API.Models.Requests;
using IMAP.API.Models.Responses;
using IMAP.Core;
using Microsoft.AspNetCore.Mvc;

namespace IMAP.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            ImapClient client = new ImapClient();
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.PassWord))
            {
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and password must be provided."
                });
            }

            await client.ConnectAsync("imap.gmail.com", 993);
            await client.AuthenticateAsync(request.UserName, request.PassWord);

            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Signed in successfully."
            });
        }
    }
}