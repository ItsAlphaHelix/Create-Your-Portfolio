
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

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
