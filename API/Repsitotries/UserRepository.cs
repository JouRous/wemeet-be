using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public UserRepository(AppDbContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    public async Task<UserDTO> GetUserAsync(string username)
    {
      return await _context.Users.Where(user => user.Username == username)
                                 .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                                 .SingleOrDefaultAsync();
    }

    public Task<bool> SaveAllAsync()
    {
      throw new System.NotImplementedException();
    }
  }
}