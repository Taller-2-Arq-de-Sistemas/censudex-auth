
using System.Collections.Concurrent;

namespace censudex_auth_service.src.Repositories
{
    /// <summary>
    /// Provides an in-memory implementation of the token block list repository for JWT token revocation.
    /// </summary>
    /// <remarks>
    /// This implementation uses a concurrent dictionary to store blocked tokens in memory.
    /// It is suitable for development, testing, or single-instance deployments but should not
    /// be used in production multi-server environments due to lack of persistence and distribution.
    /// 
    /// Features:
    /// - Thread-safe operations using <see cref="ConcurrentDictionary{TKey, TValue}"/>
    /// - Automatic cleanup of expired tokens during read operations
    /// - Fast in-memory lookups for token validation
    /// </remarks>
    public class InMemoryTokenBlockListRepository : ITokenBlockListRepository
    {
        private readonly ConcurrentDictionary<string, DateTime> _blockedTokens = new();

        /// <summary>
        /// Adds a token to the in-memory block list using its JWT ID and expiration time.
        /// </summary>
        /// <param name="jti">The JWT ID (JTI) claim value that uniquely identifies the token.</param>
        /// <param name="expiration">The expiration time of the token from the 'exp' claim.</param>
        /// <returns>A completed task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method stores the token identifier with its expiration time in a thread-safe
        /// concurrent dictionary. The token will be automatically removed during cleanup
        /// once it has expired.
        /// 
        /// Performance: O(1) average case for dictionary insertion.
        /// </remarks>
        /// <example>
        /// <code>
        /// await repository.BlockTokenAsync("a1b2c3d4-1234-5678-90ab-cdef12345678", 
        ///     DateTime.UtcNow.AddHours(1));
        /// </code>
        /// </example>
        public Task BlockTokenAsync(string jti, DateTime expiration)
        {
            _blockedTokens[jti] = expiration;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if a token is blocked by its JWT ID and performs automatic cleanup of expired tokens.
        /// </summary>
        /// <param name="jti">The JWT ID (JTI) claim value to check against the block list.</param>
        /// <returns>
        /// A task that represents the asynchronous check operation. The result is <c>true</c>
        /// if the token is currently blocked and not expired; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method:
        /// 1. First performs cleanup of any expired tokens in the block list
        /// 2. Checks if the provided JTI exists in the block list
        /// 3. Returns true only if the token is found and has not yet expired
        /// 4. Automatically removes expired tokens during the check operation
        /// 
        /// Performance: O(n) for cleanup (where n is number of blocked tokens) + O(1) for lookup.
        /// </remarks>
        /// <example>
        /// <code>
        /// var isBlocked = await repository.IsTokenBlockedAsync("a1b2c3d4-1234-5678-90ab-cdef12345678");
        /// if (isBlocked) 
        /// {
        ///     // Token is revoked, reject the request
        /// }
        /// </code>
        /// </example>
        public Task<bool> IsTokenBlockedAsync(string jti)
        {
            CleanUpExpiredTokens();

            if (_blockedTokens.TryGetValue(jti, out var expiresAt))
            {
                if (DateTime.UtcNow <= expiresAt)
                    return Task.FromResult(true);

                // Token expired, remove it
                _blockedTokens.TryRemove(jti, out _);
            }

            return Task.FromResult(false);
        }

        /// <summary>
        /// Removes all expired tokens from the in-memory block list.
        /// </summary>
        /// <remarks>
        /// This method iterates through all blocked tokens and removes those whose
        /// expiration time has passed. It is called automatically during token validation
        /// checks to maintain the cleanliness of the block list.
        /// 
        /// Note: This operation runs synchronously and blocks the current thread.
        /// In high-traffic scenarios, consider implementing a background cleanup process.
        /// </remarks>
        private void CleanUpExpiredTokens()
        {
            var now = DateTime.UtcNow;
            var expiredTokens = _blockedTokens
                .Where(kvp => kvp.Value <= now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredTokens)
                _blockedTokens.TryRemove(key, out _);
        }
    }
}