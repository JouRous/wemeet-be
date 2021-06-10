using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Models;
using API.Types;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    Task<UserDTO> GetUserAsync(string username);
    Task<Pagination<UserDTO>> GetUsersAsync(PaginationParams paginationParams);
  }
}