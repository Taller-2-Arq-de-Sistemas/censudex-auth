using censudex_auth_service.src.Extensions;
using censudex_auth_service.src.Repositories;
using censudex_auth_service.src.Services;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set");
var clientsUrl = Environment.GetEnvironmentVariable("CLIENTS_SERVICE_URL");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenBlockListRepository, InMemoryTokenBlockListRepository>();
builder.Services.ConfigureJwtAuthentication(jwtSecret);

var app = builder.Build();

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
