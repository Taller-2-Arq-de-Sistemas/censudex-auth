
using System.Security.Claims;

namespace censudex_auth_service.src.Services
{
    public interface IJwtTokenService
    {

        string GenerateToken(Guid userId, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }
}