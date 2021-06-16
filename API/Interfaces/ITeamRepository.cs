using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Models;
using API.Types;

namespace API.Interfaces
{
  public interface ITeamRepository
  {
    Task<TeamDTO> GetTeamAsync(string teamId);
    Task<Pagination<TeamDTO>> GetAllAsync(PaginationParams paginationParams);
    void AddTeam(Team team);
  }
}
