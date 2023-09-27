using SEM_project.Models;

namespace SEM_project.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}