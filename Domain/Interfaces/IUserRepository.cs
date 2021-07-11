using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using Domain.Types;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserEntityAsync(Guid id);
        Task<UserWithTeamUsersDTO> GetUserAsync(Guid id);
        Task<IEnumerable<AdminUserDTO>> GetUserAdminsAsync();
        Task<string> GetEmailAsync(Guid id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<AppUser> UpdateUserAsync(AppUser user, Guid id);
        Task<Pagination<UserWithTeamDTO>> GetUsersAsync(Query<UserFilterModel> query);
        void DeactivateUser(AppUser user);
        void RetrieveUser(AppUser user);
    }
}