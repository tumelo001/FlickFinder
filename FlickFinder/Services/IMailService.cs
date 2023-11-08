namespace FlickFinder.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string email, string subject, string type, Dictionary<string, string> emailOptions);
    }
}
