namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Errors;
    using Portfolio.API.Services.Contracts;
    using Portfolio.API.Services.Dtos;
    using Portfolio.API.Services.Dtos.AccountsDtos;
    using Portfolio.API.Services.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Net;

    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegisterDto userRegisterModel)
        {
            _ = new IdentityResult();
            IdentityResult result;
            
            try
            {
                result = await accountsService.RegisterUserAsync(userRegisterModel);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(result);

            //var user = await accountsService.GetUserByEmail(userRegisterModel.Email);

            //var emailConfirmationToken = await accountsService.GenerateConfirmationEmailToken(user);

            //await emailService.SendConfirmationMail(emailConfirmationToken, user);

            //var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));

            //return StatusCode(201, new { encodedEmail });
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginDto userLoginModel)
        {
            var user = new ApplicationUserLoginResponseDto();

            try
            {
                user = await accountsService.AuthenticateUserAsync(userLoginModel);
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

        [HttpGet]
        [AllowAnonymous]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RefreshAccessToken([FromQuery] string refreshToken, [FromQuery] string userId)
        {
            var result = new ApplicationUserTokensDto();

            try
            {
                result = await accountsService.RefreshAccessTokenAsync(refreshToken, userId);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (MemberAccessException)
            {
                return Forbid();
            }

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromQuery] string userId)
        {
            var user = await this.accountsService.GetUserAsync(userId);

            if (user == null)
            {
                return BadRequest(userId);
            }

            return Ok(user);
        }
    }
}
