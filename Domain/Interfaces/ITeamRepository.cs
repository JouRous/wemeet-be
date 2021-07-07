using System;
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
        Task<TeamWithUserDTO> GetTeamAsync(Guid teamId);
        Task<Pagination<TeamWithUserDTO>> GetAllAsync(Query<FilterTeamModel> query);
        Task AddTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task AddUserToTeamAsync(Guid teamId, ICollection<int> userIds);
        Task RemoveUserFromTeam(Guid teamId, ICollection<int> userIds);
    }
}
