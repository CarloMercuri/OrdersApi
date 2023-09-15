using OrdersApi.Authentication.Interfaces;
using OrdersApi.Data.Database.DataQueries;
using OrdersApi.Data.Database.Interfaces;
using OrdersApi.Encryption;
using OrdersApi.Encryption.Interfaces;
using OrdersApi.Security.Authentication;
using OrdersApi.Security.Authorization;
using OrdersApi.Security.Authorization.Interfaces;
using OrdersApi.Security.Database;
using OrdersApi.Security.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// Dependency injections
//

// Encryption
builder.Services.AddSingleton<IEncryptionProcessor, CEncryptionProcessor>();
builder.Services.AddSingleton<IEncryptionAgent, CEncryptor>();
builder.Services.AddSingleton<IAuthenticatorProcess, AuthenticationProcessor>();
builder.Services.AddSingleton<ISecurityDatabaseConnections, SecurityDatabaseConnection>();
builder.Services.AddSingleton<ISecurityDatabaseQueries, SecurityDatabaseQueries>();
builder.Services.AddSingleton<IAuthorizationProcessor, AuthorizationProcessor>();

// Database
builder.Services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
