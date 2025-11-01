namespace censudex_auth_service.src.Dtos.Response
{
    public class ValidateTokenResponse
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = "0";
        
    }
}