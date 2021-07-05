using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        string CreateResetPasswordToken(string email);
    }
}