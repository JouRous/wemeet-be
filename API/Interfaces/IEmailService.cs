using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IEmailService
    {
        Task sendMailAsync(string to, string subject, string html);
    }

}
