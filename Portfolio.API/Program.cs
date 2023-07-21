using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Data;
using Portfolio.API.Data.Models;
using Portfolio.API.Services.AccountService;
using System.Text;
using Portfolio.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PortfolioDatabaseConnection");

builder.Services.AddDbContext<PortfolioDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PortfolioDBContext>();

var secretKey = builder.Configuration["SecretKey"];
var jwtKey = Encoding.ASCII.GetBytes("aZmN7icOxwrKG7Hc8Pg5Kup/KilJnHd859QFGhSSnjI4ipw94N4pndJ6MK7tUVBScoUyjKiGcIIYh/CjOxN/aMBKB/gG/TsZjClx0umv2cM+B5rcULlF123QwFdI+QfwHfPcSmzxUO6xAWFsBuWoJ9r3iSutjgjAswJYvkxIj20LKc7+bNQq3CBY6waaCHVIgWf0vhE/RT2+gN5XZyOGqOPzPk6Yivo0JhylOC+L/XzkwITdzmtxPvtUNzkg5Sd2NV24Hf6Eo/90/UNduSg56O5bj4alVUsaTNc0w4/1fDpibZIR4g9kgA8FN4CMyX7MRxnULNY3Orya+MPEaudm0w==");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

const string angularAppUrl = "http://localhost:4200";

app.UseCors(option => option.WithOrigins(angularAppUrl)
.AllowAnyHeader()
.AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
