using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RaayPoll.API.DTOs.Requests;
using System.Security.Claims;
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _authService.ValidateTokenAndGenerateNewAsync(refreshTokenRequest.AccessToken, refreshTokenRequest.RefreshToken);

            if (result is null)
                return BadRequest(new
                {
                    Msg = "Invalid client request"
                });

            return Ok(result);
        }

        [HttpPut("revoke-refresh-token")]
        public async Task<IActionResult> RevokeAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _authService.RevokeTokenAsync(refreshTokenRequest.AccessToken, refreshTokenRequest.RefreshToken);

            if (!result)
                return BadRequest(new
                {
                    Msg = "Invalid client request"
                });

            return NoContent();
        }
    }
}
