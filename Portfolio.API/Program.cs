using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Data;
using Portfolio.API.Data.Models;
using Portfolio.API.ExceptionMiddlewares;
using Portfolio.API.Services.AccountService;
using Portfolio.API.Services.AccountsService;
using Portfolio.Data.Repositories;
using Portfolio.API.Services.EmailService.EmailSender;
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
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PortfolioDBContext>();

var secretKey = builder.Configuration["ApiKeys:JWTKey"];
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

//var sendGridKey = builder.Configuration["ApiKeys:SendGridKey"];
//var sendGrid = new SendGridEmailSender(sendGridKey);
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
//builder.Services.AddScoped(x => new SendGridEmailSender(sendGridKey));

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

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
