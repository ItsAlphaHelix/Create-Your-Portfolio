namespace Portfolio.API.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.UserImages = new HashSet<UserImage>();

            this.UserProgramLanguages = new HashSet<UserProgramLanguage>();
        }

        [Required]
        [MaxLength(30, ErrorMessage = "The first name length shouldn't be more than 30 symbols.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The last name length shouldn't be more than 30 symbols.")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The job title shouln't be more than 30 symbols.")]
        public string JobTitle { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<UserImage> UserImages { get; set; }

        public ICollection<UserProgramLanguage> UserProgramLanguages { get; set; }
    }
}
