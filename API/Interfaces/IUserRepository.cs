using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using API.Types;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserEntityAsync(int id);
        Task<UserWithTeamUsersDTO> GetUserAsync(int id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<AppUser> UpdateUserAsync(AppUser user, int id);
        Task<Pagination<UserWithTeamDTO>> GetUsersAsync(Query<UserFilterModel> query);
        void DeactivateUser(AppUser user);
        void RetrieveUser(AppUser user);
    }
}