
namespace censudex_auth_service.src.Dtos.Response
{
    public class ClientResponse
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required int Role { get; set; }
    public required bool IsActive { get; set; }
}

}