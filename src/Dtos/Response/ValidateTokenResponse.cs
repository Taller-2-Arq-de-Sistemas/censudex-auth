
namespace censudex_auth_service.src.Dtos.Response
{
    /// <summary>
    /// Represents the response from JWT token validation in the Censudex system.
    /// </summary>
    /// <remarks>
    /// This DTO contains the extracted claims and user information from a validated JWT token.
    /// It is used by the token validation endpoint to provide user context to consuming services.
    /// </remarks>
    public class ValidateTokenResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the authenticated user.
        /// </summary>
        /// <value>
        /// A GUID that uniquely identifies the user who owns the validated token.
        /// This is extracted from the 'sub' claim or custom identifier claim in the JWT.
        /// </value>
        /// <example>11111111-1111-1111-1111-111111111111</example>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the role of the authenticated user.
        /// </summary>
        /// <value>
        /// A string representing the user's role and permissions level.
        /// Default value is "0" (regular user). Common values: "0" = User, "1" = Administrator.
        /// </value>
        /// <remarks>
        /// Role definitions:
        /// - "0": Regular user with standard permissions
        /// - "1": Administrator with full system access
        /// This value is extracted from the role claim in the JWT token.
        /// </remarks>
        /// <example>"1"</example>
        public string Role { get; set; } = "0";
    }
}