namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class AboutUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The education length shouln't be more than 30 characters.")]
        public string Education { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The country name length shouln't be more than 30 characters.")]
        public string Country { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The city name length shouln't be more than 30 characters.")]
        public string City { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "The text you entered must not contain more than 200 characters.")]
        public string About { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }


        public ApplicationUser User { get; set; }
    }
}
