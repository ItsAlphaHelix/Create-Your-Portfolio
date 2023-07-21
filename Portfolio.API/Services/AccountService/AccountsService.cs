namespace Portfolio.API.Services.AccountService
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Dtos;
    using Portfolio.API.Services.Dtos.AccountDtos;
    using Portfolio.API.Services.Models;
    using Portfolio.Data.Repositories;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountsService : IAccountsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IRepository<ApplicationUser> userRepository;

        public AccountsService
            (
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IRepository<ApplicationUser> userRepository)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        public async Task<ApplicationUserLoginResponseDto> AuthenticateUser(ApplicationUserLoginDto user)
        {
            var findUser = userManager.FindByEmailAsync(user.Email).GetAwaiter().GetResult();

            if (findUser == null)
            {
                throw new NullReferenceException("The user can not be null");
            }

            //var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(findUser);

            //if (!isEmailConfirmed)
            //{
            //    throw new InvalidOperationException(EMAIL_NOT_CONFIRMED);
            //}

            var isAuthenticated = await userManager.CheckPasswordAsync(findUser, user.Password);
            if (!isAuthenticated)
            {
                throw new MemberAccessException("The credentials are wrong.");
            }

            await GenerateTokens(findUser);

            var authenticatedUser = new ApplicationUserLoginResponseDto()
            {
                Email = findUser.Email,
                AccessToken = findUser.AccessToken,
                RefreshToken = findUser.RefreshToken
            };

            return authenticatedUser;
        }

        public async Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto applicationUser)
        {
            var user = new ApplicationUser()
            {
                UserName = applicationUser.Username,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email
            };

            var registeredUser = await userManager.CreateAsync(user, applicationUser.Password);

            return registeredUser;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user;
        }
        private async Task GenerateTokens(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1.5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.AccessToken = tokenHandler.WriteToken(token);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1.5);

            userRepository.Update(user);
            await userRepository.SaveChangesAsync();


            //var key = Encoding.ASCII.GetBytes
            //(configuration["SecretKey"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //    new Claim("Id", Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            //    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
            //    new Claim(JwtRegisteredClaimNames.Jti,
            //    Guid.NewGuid().ToString())
            // }),
            //    Expires = DateTime.UtcNow.AddHours(1.5),
            //    SigningCredentials = new SigningCredentials
            //    (new SymmetricSecurityKey(key),
            //    SecurityAlgorithms.HmacSha512Signature)
            //};
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //user.AccessToken = tokenHandler.WriteToken(token);
            //user.RefreshToken = GenerateRefreshToken();
            //user.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1.5);

            //userRepository.Update(user);
            //await userRepository.SaveChangesAsync();
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
