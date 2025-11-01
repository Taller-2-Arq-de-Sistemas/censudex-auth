
using censudex_auth_service.src.Dtos.Request;

namespace censudex_auth_service.src.Services
{
    public interface IAuthService
    {
        Task<LoginResult> AuthenticateAsync(LoginRequest request);

    }
}