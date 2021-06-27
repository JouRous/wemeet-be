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
    Task<UserWithTeamDTO> GetUserAsync(int id);
    Task<UserDTO> FindByEmail(string email);
    Task<AppUser> UpdateUserAsync(AppUser user, int id);
    Task<Pagination<UserWithTeamDTO>> GetUsersAsync(Dictionary<string, int> page, Dictionary<string, string> filter, Dictionary<string, string> sort);
    void DeactivateUser(AppUser user);
    void RetrieveUser(AppUser user);
  }
}