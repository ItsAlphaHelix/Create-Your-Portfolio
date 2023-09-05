namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class UserImage
    {
        [Key]
        public int Id { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? HomePageImageUrl { get; set; }

        public string? AboutImageUrl { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
