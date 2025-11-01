
using System.Text;
using System.Text.Json;
using censudex_auth_service.src.Dtos.Request;
using censudex_auth_service.src.Dtos.Response;

namespace censudex_auth_service.src.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJwtTokenService _jwtService;
        private readonly string _clientsBaseUrl;

        public AuthService(HttpClient httpClient, IJwtTokenService jwtService, IConfiguration config)
        {
            _httpClient = httpClient;
            _jwtService = jwtService;
            _clientsBaseUrl = config["CLIENTS_SERVICE_URL"] ?? throw new InvalidOperationException("CLIENTS_SERVICE_URL environment variable is not set");
        }

        public async Task<LoginResult> AuthenticateAsync(LoginRequest request)
        {
            var url = $"{_clientsBaseUrl}/clients/credentials";
            var body = string.Empty;

            if (request.Identifier.Contains("@"))
            {
                body = JsonSerializer.Serialize(new
                {
                    email = request.Identifier,
                    password = request.Password
                });
            }
            else
            {
                body = JsonSerializer.Serialize(new
                {
                    username = request.Identifier,
                    password = request.Password
                });
            }

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return LoginResult.Fail("Usuario no encontrado.");

            var client = await response.Content.ReadFromJsonAsync<ClientResponse>();
            if (client == null || !client.IsActive)
                return LoginResult.Fail("Usuario inactivo o no encontrado.");

            var token = _jwtService.GenerateToken(client.Id, client.Role.ToString());
            return LoginResult.Success(token);
        }
    }
}