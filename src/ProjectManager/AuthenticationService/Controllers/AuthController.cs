using AuthenticationService.Contracts;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("auth/")]
    public class AuthController : Controller
    {
        private readonly IAuthenticator _auth;

        public AuthController(IAuthenticator auth)
        {
            _auth = auth;
        }

        [HttpPost("user")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequest request)
        {
            var token = await _auth.AuthenticateUser(request);

            Console.WriteLine(JsonConvert.SerializeObject(request));
            Console.WriteLine(JsonConvert.SerializeObject(token));

            if (token is null)
                return StatusCode(StatusCodes.Status401Unauthorized, "Incorrect email or password");

            return Ok(token.TokenId.ToString());
        }

        [HttpPost("token")]
        public async Task<IActionResult> AuthenticateToken([FromBody] TokenRequest request)
        {
            var token = await _auth.AuthenticateToken(request);

            if (token is null)
                return StatusCode(StatusCodes.Status401Unauthorized, "Incorrect token");

            return Ok(token.TokenId.ToString());
        }

        [HttpPost("getUser")]
        public async Task<IActionResult> GetUser([FromBody] TokenRequest request)
        {
            var userId = await _auth.GetUser(request);

            if (userId is null)
                return StatusCode(StatusCodes.Status404NotFound, "User not found");

            return Ok(userId.ToString());
        }

        [HttpGet()]
        public IActionResult Status() => Ok("OK");
    }
}
