
namespace censudex_auth_service.src.Dtos.Request
{
    /// <summary>
    /// Represents the result of a user authentication attempt in the Censudex system.
    /// </summary>
    /// <remarks>
    /// This class provides a standardized way to return authentication results,
    /// including success status, JWT token on successful authentication, and error messages on failure.
    /// </remarks>
    public class LoginResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the authentication attempt was successful.
        /// </summary>
        /// <value>
        /// <c>true</c> if authentication succeeded; otherwise, <c>false</c>.
        /// </value>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the JWT token generated upon successful authentication.
        /// </summary>
        /// <value>
        /// A JSON Web Token string that can be used for subsequent authenticated requests.
        /// This property is only populated when <see cref="Succeeded"/> is <c>true</c>.
        /// </value>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the error message describing why authentication failed.
        /// </summary>
        /// <value>
        /// A descriptive error message indicating the reason for authentication failure.
        /// This property is only populated when <see cref="Succeeded"/> is <c>false</c>.
        /// </value>
        public string? Error { get; set; }

        /// <summary>
        /// Creates a successful authentication result with the provided JWT token.
        /// </summary>
        /// <param name="token">The JWT token generated for the authenticated user.</param>
        /// <returns>
        /// A <see cref="LoginResult"/> instance with <see cref="Succeeded"/> set to <c>true</c>
        /// and the <see cref="Token"/> property populated.
        /// </returns>
        /// <example>
        /// <code>
        /// var result = LoginResult.Success("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...");
        /// </code>
        /// </example>
        public static LoginResult Success(string token) => new() { Succeeded = true, Token = token };

        /// <summary>
        /// Creates a failed authentication result with the specified error message.
        /// </summary>
        /// <param name="error">The error message describing the authentication failure.</param>
        /// <returns>
        /// A <see cref="LoginResult"/> instance with <see cref="Succeeded"/> set to <c>false</c>
        /// and the <see cref="Error"/> property populated.
        /// </returns>
        /// <example>
        /// <code>
        /// var result = LoginResult.Fail("Credenciales inválidas");
        /// var result = LoginResult.Fail("Usuario no encontrado");
        /// </code>
        /// </example>
        public static LoginResult Fail(string error) => new() { Succeeded = false, Error = error };
    }
}