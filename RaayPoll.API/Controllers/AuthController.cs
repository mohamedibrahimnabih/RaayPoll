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
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await _authService.ValidateAndGenerateTokenAsync(loginRequest.Email, loginRequest.Password);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    }
                );
            }

            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _authService.ValidateTokenAndGenerateNewAsync(refreshTokenRequest.AccessToken, refreshTokenRequest.RefreshToken);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    });
            }

            return Ok(result.Value);
        }

        [HttpPut("revoke-refresh-token")]
        public async Task<IActionResult> Revoke(RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _authService.RevokeTokenAsync(refreshTokenRequest.AccessToken, refreshTokenRequest.RefreshToken);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    });
            }

            return NoContent();
        }
    }
}
