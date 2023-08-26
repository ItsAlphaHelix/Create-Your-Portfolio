using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Data.Models
{
    public class UserImage
    {
        [Key]
        public int Id { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? HomePageImageUrl { get; set; }

        public string? AboutImageUrl { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
