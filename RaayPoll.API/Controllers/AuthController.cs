using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RaayPoll.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.ValidateAndGenerateToken(loginRequest.Email, loginRequest.Password, cancellationToken);

            if(result is null)
                return NotFound(new
                {
                    Msg = "Invalid Email / Password"
                });

            return Ok(result);
        }
    }
}
