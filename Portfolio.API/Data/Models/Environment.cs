namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Environment
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
