using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Models;
using API.Types;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    Task<AppUser> FindById(int id);
    Task<UserDTO> GetUserAsync(string username);
    Task<AppUser> UpdateUserAsync(AppUser user);
    Task<Pagination<UserDTO>> GetUsersAsync(Dictionary<string, int> page, Dictionary<string, string> filter, Dictionary<string, string> sort);
    void DeactivateUser(AppUser user);
    void RetrieveUser(AppUser user);
  }
}