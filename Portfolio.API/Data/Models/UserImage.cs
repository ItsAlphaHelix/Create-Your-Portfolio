namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class UserImage
    {
        [Key]
        public int Id { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string?  HomePageImageUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
