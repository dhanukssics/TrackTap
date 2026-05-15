namespace TrackTap.Repository
{
    using System.Net;
    using System.Net.Mail;

    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(
            string toEmail,
            string subject,
            string body)
        {
            var smtp =
                new SmtpClient(
                    _configuration["MailSettings:Host"])
                {
                    Port =
                        Convert.ToInt32(
                            _configuration["MailSettings:Port"]),

                    Credentials =
                        new NetworkCredential(
                            _configuration["MailSettings:Mail"],
                            _configuration["MailSettings:Password"]),

                    EnableSsl = true
                };

            var message =
                new MailMessage
                {
                    From =
                        new MailAddress(
                            _configuration["MailSettings:Mail"]),

                    Subject = subject,

                    Body = body,

                    IsBodyHtml = true
                };

            message.To.Add(toEmail);

            await smtp.SendMailAsync(message);
        }
    }
}
