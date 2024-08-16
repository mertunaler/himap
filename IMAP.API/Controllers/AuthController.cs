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
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.PassWord))
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Email and password must be provided."
                });

            ImapClient client = new ImapClient();
            bool authResult = await client.AuthenticateAsync(request.UserName, request.PassWord);

            if (authResult)
                return Ok(new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                });
            else
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
        }
    }
}
