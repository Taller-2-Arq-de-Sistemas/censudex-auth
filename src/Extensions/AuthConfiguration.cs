
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace censudex_auth_service.src.Extensions
{
    
    /// <summary>
    /// Provides extension methods for configuring JWT authentication in the Censudex authentication service.
    /// </summary>
    /// <remarks>
    /// This static class contains methods to set up JWT Bearer authentication with custom configuration
    /// for token validation, security parameters, and authentication events.
    /// </remarks>
    public static class AuthConfiguration
    {

        /// <summary>
        /// Configures JWT Bearer authentication for the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the authentication services to.</param>
        /// <param name="jwtSecret">The secret key used for signing and validating JWT tokens.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="jwtSecret"/> is null or empty.
        /// </exception>
        /// <remarks>
        /// This method sets up JWT authentication with the following configuration:
        /// <list type="bullet">
        /// <item>
        /// <description>Validates token signing key against the provided secret</description>
        /// </item>
        /// <item>
        /// <description>Validates token lifetime (expiration)</description>
        /// </item>
        /// <item>
        /// <description>Disables issuer and audience validation</description>
        /// </item>
        /// <item>
        /// <description>Sets clock skew to zero for strict expiration validation</description>
        /// </item>
        /// <item>
        /// <description>Adds event handlers for authentication logging</description>
        /// </item>
        /// </list>
        /// The JWT secret should be stored securely in environment variables and should be at least 32 characters long.
        /// </remarks>
        /// <example>
        /// <code>
        /// // In Program.cs
        /// var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        /// services.ConfigureJwtAuthentication(jwtSecret);
        /// </code>
        /// </example>
        public static void ConfigureJwtAuthentication(this IServiceCollection services, string jwtSecret)
        {
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new ArgumentNullException(nameof(jwtSecret), "Environment variable JWT_SECRET cannot be null or empty");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    // Events for better logging/debugging
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token successfully validated");
                            return Task.CompletedTask;
                        }
                    };
                });
        }

    }
}