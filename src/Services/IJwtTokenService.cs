
using System.Security.Claims;

namespace censudex_auth_service.src.Services
{
    /// <summary>
    /// Defines the contract for JWT token generation and validation services.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A JWT token string.</returns>
        string GenerateToken(Guid userId, string role);

        /// <summary>
        /// Validates a JWT token and returns the principal if valid.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>The claims principal if the token is valid; otherwise, null.</returns>
        ClaimsPrincipal? ValidateToken(string token);
    }
}