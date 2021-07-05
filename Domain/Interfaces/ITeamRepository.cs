using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using Domain.Types;

namespace Domain.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamWithUserDTO> GetTeamAsync(int teamId);
        Task<Pagination<TeamWithUserDTO>> GetAllAsync(Query<FilterTeamModel> query);
        Task AddTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task AddUserToTeamAsync(int teamId, ICollection<int> userIds);
        Task RemoveUserFromTeam(int teamId, ICollection<int> userIds);
    }
}