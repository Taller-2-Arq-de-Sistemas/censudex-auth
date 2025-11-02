
using System.Collections.Concurrent;

namespace censudex_auth_service.src.Repositories
{
    public class InMemoryTokenBlockListRepository : ITokenBlockListRepository
    {
        private readonly ConcurrentDictionary<string, DateTime> _blockedTokens = new();

        // Block token with its expiration time
        public Task BlockTokenAsync(string jti, DateTime expiration)
        {
            _blockedTokens[jti] = expiration;
            return Task.CompletedTask;
        }

        // Check if token is blocked (and clean up expired ones)
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