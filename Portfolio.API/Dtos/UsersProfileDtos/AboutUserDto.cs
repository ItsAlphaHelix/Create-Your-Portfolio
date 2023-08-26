using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Portfolio.API.Dtos.UsersProfileDtos
{
    public class AboutUserDto
    {
        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The education must be between 3 and 30 characters.")]
       // [RegularExpression(@"[A-Z]{1}[\w\d]+", ErrorMessage = "The education should start with capital letter")]
        public string Education { get; set; }

        [Required]
        [RegularExpression(@"[A-Z]{1}[\w]+", ErrorMessage = "The country should start with capital letter, and it should contain only letters.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The country name must be between 3 and 30 characters.")]
        public string Country { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The city name must be betweem 3 and 30 characters.")]
        [RegularExpression(@"[A-Z]{1}[\w]+", ErrorMessage = $"The City should start with capital letter, and it should contain only letters.")]
        public string City { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The text you entered must be more than 10 characters")]
        public string AboutMessage { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
