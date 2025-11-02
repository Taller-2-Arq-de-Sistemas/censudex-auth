
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace censudex_auth_service.src.Services
{
    /// <summary>
    /// Provides JWT token generation and validation services.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly string _secret;
        private readonly int _expiryMinutes;

        /// <summary>
        /// Initializes a new instance of the JwtTokenService class.
        /// </summary>
        /// <param name="config">The configuration containing JWT settings.</param>
        public JwtTokenService(IConfiguration config)
        {
            _secret = config["JWT_SECRET"] ?? throw new Exception("Missing JWT_SECRET in env");
            _expiryMinutes = int.Parse(config["JWT_EXPIRATION_MINUTES"] ?? "60");
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A JWT token string.</returns>
        public string GenerateToken(Guid userId, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jti = Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("Role", role),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Validates a JWT token and returns the principal if valid.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>The claims principal if the token is valid; otherwise, null.</returns>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            try
            {
                return tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}