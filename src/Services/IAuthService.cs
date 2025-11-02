using censudex_auth_service.src.Dtos.Request;

namespace censudex_auth_service.src.Services
{
    /// <summary>
    /// Defines the contract for authentication operations in the Censudex system.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user using their credentials.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>A <see cref="LoginResult"/> indicating authentication success or failure.</returns>
        Task<LoginResult> AuthenticateAsync(LoginRequest request);
    }
}