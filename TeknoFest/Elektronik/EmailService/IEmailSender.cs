using System.Threading.Tasks;

namespace ElektronikWebUI.EmailService
{
    public interface IEmailSender
    {

        Task SendEmailAsync(string email,string subject,string htmlMessage);

    }
}
