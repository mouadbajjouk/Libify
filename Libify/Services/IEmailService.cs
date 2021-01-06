using Libify.Models;
using System.Threading.Tasks;

namespace Libify.Services
{
    public interface IEmailService
    {
        Task SendForgotPasswordEmail(SendEmailOptions sendEmailOptions);

        Task SendEmailForEmailConfirmation(SendEmailOptions sendEmailOptions);
    }
}