using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Models;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    Task<bool> SaveAllAsync();
    Task<UserDTO> GetUserAsync(string username);
    Task<Pagination<UserDTO>> GetUsersAsync();
  }
}