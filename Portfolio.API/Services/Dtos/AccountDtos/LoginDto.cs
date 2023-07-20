namespace Portfolio.API.Services.Dtos
{
    using System.ComponentModel.DataAnnotations;
    public class LoginDto
    {
        //[EmailAddress]
        //[Required(ErrorMessage = "Email is required.")]
        //public string Email { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password must be the same.")]
        [Required(ErrorMessage = "Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
