using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Interfaces;
using API.Models;
using API.Services;
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
      return await _context.Users.Where(user => user.UserName == username)
                                 .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                                 .SingleOrDefaultAsync();
    }

    public async Task<Pagination<UserDTO>> GetUsersAsync()
    {
      var query = _context.Users.ProjectTo<UserDTO>(_mapper.ConfigurationProvider).AsQueryable();

      return await PaginationService.GetPagination<UserDTO>(query, 20, 20);
    }

    public Task<bool> SaveAllAsync()
    {
      throw new System.NotImplementedException();
    }

  }
}