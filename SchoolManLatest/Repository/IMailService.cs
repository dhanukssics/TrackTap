namespace TrackTap.Repository
{
    public interface IMailService
    {
        Task SendAsync(
            string toEmail,
            string subject,
            string body);
    }
}
