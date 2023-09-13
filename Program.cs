using OrdersApi.Data.Database.DataQueries;
using OrdersApi.Data.Database.Interfaces;
using OrdersApi.Encryption;
using OrdersApi.Encryption.Interfaces;

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
