namespace Portfolio.API.Services.Register
{
    using Portfolio.API.Services.Dtos;
    using System.IdentityModel.Tokens.Jwt;

    public interface ILoginService
    {
        Task<string> LoginAsync(LoginDto model);
    }
}
