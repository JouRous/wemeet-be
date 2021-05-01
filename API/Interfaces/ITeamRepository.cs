using System.Threading.Tasks;
using API.DTO;

namespace API.Interfaces
{
  public interface ITeamRepository
  {
    Task<bool> CreateAsync();
    Task<TeamDTO> GetTeamAsync();
  }
}