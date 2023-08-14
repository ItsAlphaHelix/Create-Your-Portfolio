namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class UserProfileImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
