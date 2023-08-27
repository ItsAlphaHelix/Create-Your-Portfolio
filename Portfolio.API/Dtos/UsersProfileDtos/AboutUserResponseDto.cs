namespace Portfolio.API.Dtos.UsersProfileDtos
{
    using System.ComponentModel.DataAnnotations;
    public class AboutUserResponseDto : AboutUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string JobTitle { get; set; }
    }
}
