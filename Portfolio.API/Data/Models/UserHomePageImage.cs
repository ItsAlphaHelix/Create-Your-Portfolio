namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class UserHomePageImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomePageImageUrl{ get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
