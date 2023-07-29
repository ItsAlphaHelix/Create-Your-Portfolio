namespace Portfolio.API.Services.Models
{
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUserRegisterDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The first name length shouldn't be less than 3 symbols.")]
        [RegularExpression(@"[A-Z]{1}[\w]+", ErrorMessage = "The first name should start with a capital letter, and it should contain only letters.")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The last name length shouldn't be less than 3 symbols.")]
        [RegularExpression(@"[A-Z]{1}[\w]+", ErrorMessage = "The last name should start with a capital letter, and it should contain only letters.")]
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
