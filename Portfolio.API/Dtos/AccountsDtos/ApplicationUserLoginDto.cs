namespace Portfolio.API.Services.Dtos.AccountsDtos
{
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
