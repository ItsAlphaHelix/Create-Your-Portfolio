namespace Portfolio.API.Services.RegisterService
{
    using Microsoft.AspNetCore.Identity;
    using Portfolio.API.Data;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Models;
    public interface IRegisterService
    {
        Task<(ApplicationUser, IdentityResult)> RegisterAsync(RegisterDto model);
    }
}
