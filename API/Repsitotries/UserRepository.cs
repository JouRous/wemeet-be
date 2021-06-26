using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Models;
using API.Services;
using API.Types;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Repositories
{
  public class UserRepository : IUserRepository
  {
    public readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public UserRepository(
      AppDbContext context,
      IMapper mapper
    )
    {
      _mapper = mapper;
      _context = context;
    }

    public void DeactivateUser(AppUser user)
    {
      user.DeletedAt = DateTime.Now;
      user.isDeactivated = true;
    }

    public async Task<AppUser> FindById(int id)
    {
      var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).SingleOrDefaultAsync(u => u.Id == id);

      return user;
    }

    public async Task<UserDTO> GetUserAsync(int id)
    {
      return await _context.Users.Where(user => user.Id == id)
                                 .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                                 .SingleOrDefaultAsync();
    }

    public async Task<UserDTO> FindByEmail(string email)
    {
      return await _context.Users.Where(user => user.Email == email)
                                 .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                                 .SingleOrDefaultAsync();
    }

    public async Task<Pagination<UserDTO>> GetUsersAsync(Dictionary<string, int> page,
                                                         Dictionary<string, string> filter,
                                                         Dictionary<string, string> sort)
    {
      var filterSerializer = JsonConvert.SerializeObject(filter);
      var pageSerializer = JsonConvert.SerializeObject(page);
      var _filter = JsonConvert.DeserializeObject<UserFilterModel>(filterSerializer);
      var paginationParams = JsonConvert.DeserializeObject<PaginationParams>(pageSerializer);
      var _sort = sort.GetValueOrDefault("sort");

      var fullname = Utils.Utils.RemoveAccentedString(_filter.fullname).ToLower();

      var stat = _context.Users.Where(u =>
                            u.UnsignedName.ToLower().Contains(fullname) ||
                            u.Email.Contains(fullname))
                                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider);

      switch (_sort)
      {
        case "created_at":
          stat = stat.OrderBy(s => s.CreatedAt);
          break;
        case "-created_at":
          stat = stat.OrderByDescending(s => s.CreatedAt);
          break;
      }
      var query = stat.AsQueryable();
      return await PaginationService.GetPagination<UserDTO>(query, paginationParams.number, paginationParams.size);

    }

    public void RetrieveUser(AppUser user)
    {
      user.isDeactivated = false;
      user.DeletedAt = null;
    }

    public async Task<AppUser> UpdateUserAsync(AppUser user)
    {
      var _user = await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Email == user.Email);
      if (_user != null)
      {
        _user.Nickname = user.Nickname;
        _user.Fullname = user.Fullname;
        _user.Position = user.Position;
      }

      return _user;
    }

  }
}