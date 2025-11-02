
using censudex_auth_service.src.Extensions;
using censudex_auth_service.src.Repositories;
using censudex_auth_service.src.Services;
using DotNetEnv;

/// <summary>
/// Main entry point for the Censudex Authentication Service application.
/// </summary>
/// <remarks>
/// This application provides JWT-based authentication services for the Censudex system,
/// including user login, token validation, and logout functionality with token revocation.
/// </remarks>
Env.Load();
var builder = WebApplication.CreateBuilder(args);

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set");

/// <summary>
/// Configures the application services and dependency injection container.
/// </summary>
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Registers application services for dependency injection.
/// </summary>
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.AddSingleton<ITokenBlockListRepository, InMemoryTokenBlockListRepository>();
builder.Services.ConfigureJwtAuthentication(jwtSecret);

var app = builder.Build();

/// <summary>
/// Configures the HTTP request pipeline for the application.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();