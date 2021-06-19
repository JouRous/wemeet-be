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
    Task<UserDTO> GetUserAsync(string username);
    Task<AppUser> UpdateUserAsync(AppUser user);
    Task<Pagination<UserDTO>> GetUsersAsync(PaginationParams paginationParams);
    Task SaveResetPasswordToken(string email, string token);
    Task<bool> VerifyResetPasswordToken(AppUser user, string token);
    void DeactivateUser(AppUser user);
    void RetrieveUser(AppUser user);
  }
}