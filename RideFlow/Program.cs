using RideFlow.Models;
using RideFlow.Repositories;
using RideFlow.Service;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

Console.WriteLine($"Conectando em: {connectionString}");

// Não precisa mais mapear enums, pois são strings no banco
builder.Services.AddDbContext<RideflowContext>(options =>
    options.UseNpgsql(connectionString) // Use diretamente a connection string
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DriverRepository>();
builder.Services.AddScoped<DriverService>();
builder.Services.AddScoped<RideRepository>();
builder.Services.AddScoped<RideService>();
builder.Services.AddScoped<ServiceTypeRepository>();

var app = builder.Build();

// Verificação opcional dos service types
using (var scope = app.Services.CreateScope())
{
    var serviceTypeRepo = scope.ServiceProvider.GetRequiredService<ServiceTypeRepository>();
    var serviceTypes = serviceTypeRepo.GetAll();
    Console.WriteLine($"Service Types encontrados: {serviceTypes.Count}");
    foreach (var st in serviceTypes)
    {
        Console.WriteLine($"- {st.Category}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();