using PersonalProject.Models.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace PersonalProject.Models.Services
{
    public class EmailSenderService : IEmailSender
    {

        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string apiKey = _configuration["SendGrid:Key"];
            var client = new SendGridClient(apiKey);
            SendGridMessage msg = new SendGridMessage();

            //DefaultFromEmailAddress
            string fromEmailAddress = _configuration["SendGrid:DefaultFromEmailAddress"];
            string fromName = _configuration["SendGrid:DefaultFromName"];
            msg.SetFrom(fromEmailAddress, fromName);

            msg.AddTo(email);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, htmlMessage);

            await client.SendEmailAsync(msg);


          
        }
    
}
}
