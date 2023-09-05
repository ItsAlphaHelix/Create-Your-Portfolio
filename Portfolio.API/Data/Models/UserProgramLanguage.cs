namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class UserProgramLanguage
    {
        [Key]
        public int Id { get; set; }

        public string LanguageName { get; set; }

        public double PercentageOfUseLanguage { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
