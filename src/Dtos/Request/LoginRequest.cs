
using System.ComponentModel.DataAnnotations;

namespace censudex_auth_service.src.Dtos.Request
{
    /// <summary>
    /// Represents a data transfer object for user authentication requests in the Censudex system.
    /// </summary>
    /// <remarks>
    /// This DTO is used for handling user login attempts, accepting either username or email
    /// as the identifier along with the password for authentication.
    /// </remarks>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the user identifier for authentication.
        /// </summary>
        /// <value>
        /// The user's email address or username. This field accepts either identifier type.
        /// </value>
        /// <remarks>
        /// The system will attempt to match this value against both email and username fields
        /// in the database to find the corresponding user account.
        /// </remarks>
        [Required(ErrorMessage = "El identificador (email o nombre de usuario) es obligatorio.")]
        public string Identifier { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's password for authentication.
        /// </summary>
        /// <value>
        /// The password associated with the user account.
        /// </value>
        /// <remarks>
        /// The password will be verified against the stored bcrypt hash in the database.
        /// For security reasons, no specific password requirements are disclosed in error messages.
        /// </remarks>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = null!;
    }
}