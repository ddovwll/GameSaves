using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SavesService.Application.Contracts;
using SavesService.Application.Services;
using SavesService.Core.Contracts;
using SavesService.Infrastructure.Services;
using SavesService.Persistence;
using SavesService.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddDbContext<DbHelper>(optionsBuilder =>
{
    optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");
    //optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"));
});

builder.Services.AddScoped<IGameSaveRepository, GameSaveRepository>();
builder.Services.AddScoped<IGameService, GameSaveService>();
builder.Services.AddSingleton<IFileSystemService, FileSystemService>();
        
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "GameSavesIdentityService",
            ValidAudience = "GameSavesServices",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetValue<string>("JwtKey")!))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();

app.Run();