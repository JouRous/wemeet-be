using System.Threading.Tasks;
using API.DTO;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    Task<bool> SaveAllAsync();
    Task<UserDTO> GetUserAsync(string username);
  }
}