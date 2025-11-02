
namespace censudex_auth_service.src.Dtos.Response
{
    /// <summary>
    /// Represents a client information response in the Censudex system.
    /// </summary>
    /// <remarks>
    /// This DTO contains essential client information returned by the authentication service,
    /// typically used in token validation responses or user profile endpoints.
    /// It excludes sensitive data like passwords and personal information for security.
    /// </remarks>
    public class ClientResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the client.
        /// </summary>
        /// <value>
        /// A GUID that uniquely identifies the client in the system.
        /// </value>
        /// <example>11111111-1111-1111-1111-111111111111</example>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the client's email address.
        /// </summary>
        /// <value>
        /// The email address associated with the client's account.
        /// Must follow the censudex.cl domain format.
        /// </value>
        /// <example>admin@censudex.cl</example>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the client's username.
        /// </summary>
        /// <value>
        /// The unique username chosen by the client for authentication and identification.
        /// </value>
        /// <example>admin</example>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the client's role in the system.
        /// </summary>
        /// <value>
        /// An integer representing the client's role and permissions level.
        /// Common values: 0 = Regular user, 1 = Administrator.
        /// </value>
        /// <remarks>
        /// Role definitions:
        /// - 0: Regular client with standard permissions
        /// - 1: Administrator with full system access
        /// </remarks>
        /// <example>1</example>
        public required int Role { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client account is active.
        /// </summary>
        /// <value>
        /// <c>true</c> if the client account is active and can authenticate;
        /// <c>false</c> if the account has been deactivated or soft-deleted.
        /// </value>
        /// <remarks>
        /// Inactive accounts cannot authenticate or access protected resources.
        /// </remarks>
        /// <example>true</example>
        public required bool IsActive { get; set; }
    }
}