
using Application;
using Infastructure;
using Application.Interfaces;
using Application.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Api.Middleware;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Configure the App configuration (appsettings.json, user secrets, environment variables, etc.)

//builder.Services.AddSwaggerGen(option =>
//{

//    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
//    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter a valid token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "Bearer"
//    });
//    option.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type=ReferenceType.SecurityScheme,
//                    Id="Bearer"
//                }
//            },
//            new string[]{}
//        }
//    });
//});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuth", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
});

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
var apiKey = app.Configuration["AuthLogin:UserName"];
var logLevel = app.Configuration["AuthLogin:Password"];

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<RequestResponseMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
