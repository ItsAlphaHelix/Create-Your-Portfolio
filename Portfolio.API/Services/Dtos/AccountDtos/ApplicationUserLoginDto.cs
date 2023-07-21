using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Services.Dtos.AccountDtos
{
    public class ApplicationUserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

     //   public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
    }
}
