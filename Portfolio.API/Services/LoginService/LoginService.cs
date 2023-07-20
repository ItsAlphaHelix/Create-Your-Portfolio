namespace Portfolio.API.Services.Register
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Dtos;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public LoginService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = userManager.FindByNameAsync(model.Username).GetAwaiter().GetResult();
            var issuer = configuration["JWT:ValidIssuer"];
            var audience = configuration["JWT:ValidAudience"];
            var key = Encoding.ASCII.GetBytes
            (configuration["JWT:SecretToken"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            //var user =  userManager.FindByNameAsync(model.Username).GetAwaiter().GetResult();

            //if (user == null)
            //{
            //    throw new ArgumentException("The user can not be null.");
            //}

            //if (await userManager.CheckPasswordAsync(user, model.Password) == false)
            //{
            //    throw new ArgumentException("Incorrect password.");
            //}

            //var authClaims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    };

            //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"]));

            //var token = new JwtSecurityToken(
            //    issuer: this.configuration["JWT:ValidIssuer"],
            //    audience: this.configuration["JWT:ValidAudience"],
            //    expires: DateTime.Now.AddHours(3),
            //    claims: authClaims,
            //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //    );

            return stringToken;
        }
    }
}
