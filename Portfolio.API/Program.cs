using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Data;
using Portfolio.API.Data.Models;
using Portfolio.API.ExceptionMiddlewares;
using Portfolio.API.Extensions;
using Portfolio.API.Services;
using Portfolio.API.Services.Contracts;
using Portfolio.Data.Repositories;
using System.Net;
using System.Net.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PortfolioDatabaseConnection");

builder.Services.AddDbContext<PortfolioDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PortfolioDBContext>();

var secretKey = builder.Configuration["JWTKey"];
var jwtKey = Encoding.ASCII.GetBytes(secretKey);
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
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IAboutMeService, AboutMeService>();
builder.Services.AddScoped<IGitHubApiService, GitHubApiService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<RateLimitCheckerService>();
builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler();
app.ConfigureBuiltinExceptionHandler(app.Environment);

app.UseHttpsRedirection();

const string angularAppUrl = "http://localhost:4200";

app.UseCors(option => option.WithOrigins(angularAppUrl)
.AllowAnyHeader()
.AllowAnyMethod());

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<ExceptionMiddleware>();



//app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<RateLimitHub>("/rateLimitHub");
    endpoints.MapControllers();
});

app.Run();
