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
    Task UpdateUserAsync(AppUser user);
    Task<Pagination<UserDTO>> GetUsersAsync(PaginationParams paginationParams);
    Task SaveResetPasswordToken(string email, string token);
    Task<bool> VerifyResetPasswordToken(AppUser user, string token);
  }
}