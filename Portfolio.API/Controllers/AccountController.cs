using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Data.Models;
using Portfolio.API.Services.Dtos;
using Portfolio.API.Services.Models;
using Portfolio.API.Services.Register;
using Portfolio.API.Services.RegisterService;
using System.IdentityModel.Tokens.Jwt;

namespace Portfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILoginService loginService;
        private IRegisterService registerService;

        public AccountController(
            ILoginService loginService,
            IRegisterService registerService)
        {
            this.loginService = loginService;
            this.registerService = registerService;
        }


        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            (ApplicationUser userExists, IdentityResult result) = await this.registerService.RegisterAsync(model);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var token = await this.loginService.LoginAsync(model);

                return Ok(token);
                //return Ok(new
                //{
                //    token = new JwtSecurityTokenHandler().WriteToken(token),
                //    expiration = token.ValidTo
                //});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Unauthorized();
            }
        }
    }
}
