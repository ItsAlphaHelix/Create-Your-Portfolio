namespace Portfolio.API.Services.EmailService
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Options;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.EmailService.EmailSender;
    using System.Reflection;
    using System.Text;
    public class EmailService : IEmailService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        public EmailService(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public async Task SendConfirmationMail(string emailToken, ApplicationUser user)
        {
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));

            //var encodedUserId = dataProtector.Protect(user.Id.ToString());

            var htmlString = GenerateHtmlContent(token, user.Id, "ConfirmEmailTemplate.html", "ConfirmEmailUrl");

            await emailSender.SendEmailAsync(
                "CreateYourPortfolio22@gmail.com",
                "Create-Your-Portfolio",
                user.Email,
                "Confirm Email Address",
                htmlString);
        }

        public async Task ResendConfirmationMail(string email)
        {
            var decodedEmail = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(email));

            var user = await userManager.FindByEmailAsync(decodedEmail);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendConfirmationMail(token, user);
        }

        public async Task SendPasswordResetMail(ApplicationUser user, string passwordResetToken)
        {
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetToken));

            //var encodedEmail = dataProtector.Protect(user.Email);

            var htmlString = GenerateHtmlContent(token, user.Id.ToString(), "PasswordResetEmailTemplate.html", "ResetPasswordUrl");

            await emailSender.SendEmailAsync(
                "create-your-portfolio@gmail.com",
                "Create-Your-Portfolio",
                user.Email,
                "Reset Password Email",
                htmlString);
        }

        private string GenerateHtmlContent(string emailToken, string secondParameter, string fileName, string urlToFrontEnd)
        {
            var asm = Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(asm.Location);
            var htmlString = File.ReadAllText(path + $"/Services/EmailService/EmailSender/MailTemplates/{fileName}");

            var htmlContent = string.Format(
                htmlString,
                "https://localhost:7126/",
                urlToFrontEnd,
                emailToken,
                secondParameter);

            return htmlContent;
        }
    }
}
