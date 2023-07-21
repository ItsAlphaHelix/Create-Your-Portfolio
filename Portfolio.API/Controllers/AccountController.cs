namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.AccountService;
    using Portfolio.API.Services.Dtos;
    using Portfolio.API.Services.Dtos.AccountDtos;
    using Portfolio.API.Services.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountsService accountsService;

        public AccountController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }


        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegisterDto userRegisterModel)
        {
            var result = await accountsService.RegisterUser(userRegisterModel);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
            //var user = await accountsService.GetUserByEmail(userRegisterModel.Email);

          //var emailConfirmationToken = await accountsService.GenerateConfirmationEmailToken(user);

            //await emailService.SendConfirmationMail(emailConfirmationToken, user);

            //var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));

            //return StatusCode(201, new { encodedEmail });
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginDto userLoginModel)
        {
            var user = new ApplicationUserLoginResponseDto();

            try
            {
                user = await accountsService.AuthenticateUser(userLoginModel);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is MemberAccessException)
                {
                    return Unauthorized(e.Message);
                }
            }

            return Ok(user);
        }

    }
}
