using System.Threading.Tasks;

namespace API.Interfaces
{
  public interface IUnitOfWork
  {
    IUserRepository USerRepository { get; }

    Task<bool> Complete();
    bool HasChanges();
  }
}