using System.Threading.Tasks;

namespace API.Interfaces
{
  public interface IUnitOfWork
  {
    IUserRepository USerRepository { get; }
    ITeamRepository TeamRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
  }
}