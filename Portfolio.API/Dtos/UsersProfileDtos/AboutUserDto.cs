using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Dtos.UsersProfileDtos
{
    public class AboutUserDto
    {
        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The education must be between 3 and 30 characters.")]
        public string Education { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The country name must be between 3 and 30 characters.")]
        public string Country { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The city name must be betweem 3 and 30 characters.")]
        public string City { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The text you entered must be between 10 and 200 characters.")]
        public string About { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
