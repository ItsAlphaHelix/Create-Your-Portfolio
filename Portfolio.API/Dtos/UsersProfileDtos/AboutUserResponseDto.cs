namespace Portfolio.API.Dtos.UsersProfileDtos
{
    using System.ComponentModel.DataAnnotations;
    public class AboutUserResponseDto
    {
        public int Age { get; set; }

        public string Education { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string About { get; set; }
    }
}
