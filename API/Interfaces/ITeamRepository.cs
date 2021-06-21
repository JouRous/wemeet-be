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
    Task<TeamDTO> GetTeamAsync(int teamId);
    Task<Pagination<TeamDTO>> GetAllAsync(PaginationParams paginationParams, string filter, string sort);
    void AddTeam(Team team);
    Task UpdateTeamAsync(Team team);
    Task AddUserToTeamAsync(int teamId, ICollection<int> userIds);
    Task RemoveUserFromTeam(int teamId, ICollection<int> userIds);
  }
}
