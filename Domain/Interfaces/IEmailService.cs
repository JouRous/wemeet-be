using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailService
    {
        Task sendMailAsync(string to, string subject, string html);
    }

}
