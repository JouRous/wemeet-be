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
        Task<Pagination<UserWithTeamDTO>> GetUsersAsync(Query<UserFilterModel> query);
        Task<AppUser> GetUserEntityAsync(Guid id);
        Task<UserWithTeamUsersDTO> GetUserAsync(Guid id);
        Task<IEnumerable<AdminUserDTO>> GetUserAdminsAsync();
        Task<AppUser> GetUserEntityByEmailAsync(string email);
        Task<string> GetEmailAsync(Guid id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(AppUser user);
        Task DeactivateUser(AppUser user);
        Task RetrieveUser(AppUser user);
    }
}