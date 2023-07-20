namespace Portfolio.API.Services.RegisterService
{
    using Microsoft.AspNetCore.Identity;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Models;
    using System.Security.Cryptography;

    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RegisterService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<(ApplicationUser, IdentityResult)> RegisterAsync(RegisterDto model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);

            //GenerateJwtSecretKey(32);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await userManager.CreateAsync(user, model.Password);
            return (userExists, result);
        }

        private string GenerateJwtSecretKey(int keyLengthBytes)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[keyLengthBytes];
                rng.GetBytes(randomBytes);

                // Encode the random bytes into a Base64 string
                string jwtSecretKey = Convert.ToBase64String(randomBytes);
                return jwtSecretKey;
            }
        }
    }
}
