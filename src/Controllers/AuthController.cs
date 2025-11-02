
using System.Security.Claims;
using censudex_auth_service.src.Dtos.Request;
using censudex_auth_service.src.Dtos.Response;
using censudex_auth_service.src.Repositories;
using censudex_auth_service.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace censudex_auth_service.src.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenBlockListRepository _tokenBlockListRepository;

        public AuthController(IAuthService authService, ITokenBlockListRepository tokenBlockListRepository)
        {
            _authService = authService;
            _tokenBlockListRepository = tokenBlockListRepository;
        }

        /// POST /auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginResult = await _authService.AuthenticateAsync(request);

            if (!loginResult.Succeeded)
                return Unauthorized(loginResult.Error);

            return Ok(loginResult);
        }

        /// GET /auth/validate-token
        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateTokenAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst("Role")?.Value;
            var jti = User.FindFirst("jti")?.Value;

            if (userId is null || jti is null)
                return Unauthorized("Token inválido.");

            bool isBlocked = await _tokenBlockListRepository.IsTokenBlockedAsync(jti);
            if (isBlocked)
                return Unauthorized("Token bloqueado o sesión cerrada.");

            return Ok(new ValidateTokenResponse
            {
                UserId = Guid.Parse(userId),
                Role = userRole ?? "0"
            });
        }

        /// POST /auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var jti = User.FindFirst("jti")?.Value;
            if (jti == null)
                return BadRequest("Token no contiene un identificador único.");

            var expiration = User.FindFirst("exp")?.Value;
            if (expiration != null && long.TryParse(expiration, out var expUnix))
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                await _tokenBlockListRepository.BlockTokenAsync(jti, expirationTime);
            }

            return Ok("Sesión cerrada con éxito.");
        }

    }
}