using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using API.Types;

namespace Domain.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamWithUserDTO> GetTeamAsync(int teamId);
        Task<Pagination<TeamWithUserDTO>> GetAllAsync(Query<FilterTeamModel> query);
        void AddTeam(Team team);
        Task UpdateTeamAsync(Team team);
        Task AddUserToTeamAsync(int teamId, ICollection<int> userIds);
        Task RemoveUserFromTeam(int teamId, ICollection<int> userIds);
    }
}
