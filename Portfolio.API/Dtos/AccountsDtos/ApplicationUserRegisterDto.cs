namespace Portfolio.API.Services.Models
{
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The first name must consist of 3 to 30 letters only")]
        [RegularExpression(@"[A-Z]{1}[\w]+", ErrorMessage = "The first name should start with a capital letter, and it should contain only letters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The last name must consist of 3 to 30 letters only")]
        [RegularExpression(@"[A-Z]{1}[\w]+", ErrorMessage = "The last name should start with a capital letter, and it should contain only letters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The job title must consist of 3 to 30 letters only")]
        public string JobTitle { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,}$")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password must be the same.")]
        [Required(ErrorMessage = "Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
