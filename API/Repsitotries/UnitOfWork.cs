using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using AutoMapper;

namespace API.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private AppDbContext _context;
    private IMapper _mapper;

    public IUserRepository USerRepository => new UserRepository(_context, _mapper);
    public ITeamRepository TeamRepository => new TeamRepository(_context, _mapper);


    public UnitOfWork(AppDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<bool> Complete()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
      throw new System.NotImplementedException();
    }
  }
}