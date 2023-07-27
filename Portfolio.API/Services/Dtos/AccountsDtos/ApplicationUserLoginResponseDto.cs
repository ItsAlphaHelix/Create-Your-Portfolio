namespace Portfolio.API.Services.Dtos
{
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUserLoginResponseDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
