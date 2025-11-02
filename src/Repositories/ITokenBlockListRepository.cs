
namespace censudex_auth_service.src.Repositories
{
    public interface ITokenBlockListRepository
    {
        Task BlockTokenAsync(string jti, DateTime expiration);
        Task<bool> IsTokenBlockedAsync(string jti);
    }
}