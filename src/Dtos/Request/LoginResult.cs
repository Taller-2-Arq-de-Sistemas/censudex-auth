
namespace censudex_auth_service.src.Dtos.Request
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
        public static LoginResult Success(string token) => new() { Succeeded = true, Token = token };
        public static LoginResult Fail(string error) => new() { Succeeded = false, Error = error };
    }

}