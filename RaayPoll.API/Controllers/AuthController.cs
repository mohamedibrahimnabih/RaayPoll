using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace RaayPoll.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            var result = await _authService.ValidateAndGenerateTokenAsync(loginRequest.Email, loginRequest.Password);

            if(result is null)
                return NotFound(new
                {
                    Msg = "Invalid Email / Password"
                });

            return Ok(result);
        }
    }
}
