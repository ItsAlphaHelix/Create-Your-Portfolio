namespace Portfolio.API.Services.Models
{
    using System.ComponentModel.DataAnnotations;
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MinLength(3)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password must be the same.")]
        [Required(ErrorMessage = "Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
