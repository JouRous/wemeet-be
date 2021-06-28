using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Models;
using API.Types;

namespace API.Interfaces
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
