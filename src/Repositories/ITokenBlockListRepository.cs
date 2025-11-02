
namespace censudex_auth_service.src.Repositories
{
    /// <summary>
    /// Defines the contract for managing JWT token block list operations in the Censudex authentication service.
    /// </summary>
    /// <remarks>
    /// This repository interface provides methods to block and check the status of JWT tokens,
    /// enabling features like token revocation and logout functionality.
    /// Implementations should store token identifiers (JTI) with their expiration times
    /// for efficient validation during token verification.
    /// </remarks>
    public interface ITokenBlockListRepository
    {
        /// <summary>
        /// Adds a token to the block list using its unique JWT ID (JTI) and expiration time.
        /// </summary>
        /// <param name="jti">The JWT ID (JTI) claim value that uniquely identifies the token.</param>
        /// <param name="expiration">The expiration time of the token from the 'exp' claim.</param>
        /// <returns>A task that represents the asynchronous block operation.</returns>
        /// <remarks>
        /// This method should store the JTI along with its expiration time to allow automatic
        /// cleanup of expired tokens. The token will be considered blocked until it naturally expires.
        /// 
        /// Typical usage:
        /// - Call this method during user logout to revoke the current token
        /// - Call when a token needs to be invalidated before its expiration
        /// - The implementation should respect the expiration for automatic cleanup
        /// </remarks>
        /// <example>
        /// <code>
        /// await tokenBlockList.BlockTokenAsync("a1b2c3d4-1234-5678-90ab-cdef12345678", DateTime.UtcNow.AddHours(1));
        /// </code>
        /// </example>
        Task BlockTokenAsync(string jti, DateTime expiration);

        /// <summary>
        /// Checks whether a token is currently blocked based on its JWT ID (JTI).
        /// </summary>
        /// <param name="jti">The JWT ID (JTI) claim value to check against the block list.</param>
        /// <returns>
        /// A task that represents the asynchronous check operation. The result is <c>true</c>
        /// if the token is blocked; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method should:
        /// - Check if the JTI exists in the block list
        /// - Consider token expiration (automatically treat expired tokens as not blocked)
        /// - Return quickly as this is called during every authenticated request
        /// 
        /// Typical usage:
        /// - Called during JWT validation to reject blocked tokens
        /// - Used by authentication middleware to enforce token revocation
        /// </remarks>
        /// <example>
        /// <code>
        /// var isBlocked = await tokenBlockList.IsTokenBlockedAsync("a1b2c3d4-1234-5678-90ab-cdef12345678");
        /// if (isBlocked) 
        /// {
        ///     return Unauthorized("Token has been revoked");
        /// }
        /// </code>
        /// </example>
        Task<bool> IsTokenBlockedAsync(string jti);
    }
}