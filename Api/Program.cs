
using Application;
using Infastructure;
using Application.Interfaces;
using Application.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitofwork, Unitofwork>();
//addtransient addsingelton
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddMemoryCache();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddApplication();
builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer("Data Source=DESKTOP-O7KPC84\\MSSQLSERVER01;" +
  "Initial Catalog=Users;" + "TrustServerCertificate=True;" + "Integrated Security=True;"));
//builder.Services.AddInfrastructure(builder.Configuration);
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("logs/-",rollingInterval:RollingInterval.Day).CreateLogger();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<RequestResponseMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
