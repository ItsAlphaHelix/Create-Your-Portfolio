using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Portfolio.API.Dtos.UsersProfileDtos
{
    public class AboutUserDto
    {
        public int Id { get; set; }

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
        [MinLength(10, ErrorMessage = "The text you entered must be more than 10 characters")]
        public string AboutMessage { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
