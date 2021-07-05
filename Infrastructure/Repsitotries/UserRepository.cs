using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Application.Services;
using Domain.Types;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Application.Utils;
using Infrastructure.Data;

namespace Infrastructure.Repositories
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
            user.isActive = false;
        }

        public async Task<AppUser> GetUserEntityAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<UserWithTeamUsersDTO> GetUserAsync(int id)
        {
            return await _context.Users.Where(user => user.Id == id)
                                       .Include(u => u.AppUserTeams)
                                       .ThenInclude(t => t.Team)
                                       .ProjectTo<UserWithTeamUsersDTO>(_mapper.ConfigurationProvider)
                                       .SingleOrDefaultAsync();
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Where(user => user.Email == email)
                                       .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                                       .SingleOrDefaultAsync();
        }

        public async Task<Pagination<UserWithTeamDTO>> GetUsersAsync(Query<UserFilterModel> userQuery)
        {
            var _filter = userQuery.filter;
            var paginationParams = userQuery.paginationParams;
            var _sort = userQuery.sort;

            var fullname = StringHelper.RemoveAccentedString(_filter.fullname).ToLower();

            var stat = _context.Users.Where(u =>
                                  u.UnsignedName.ToLower().Contains(fullname) ||
                                  u.Email.Contains(fullname));

            if (!string.IsNullOrEmpty(_filter.role))
            {
                stat = stat.Where(u => u.Role.Equals(_filter.role));
            }

            switch (_sort)
            {
                case "created_at":
                    stat = stat.OrderBy(s => s.CreatedAt);
                    break;
                case "-created_at":
                    stat = stat.OrderByDescending(s => s.CreatedAt);
                    break;
            }
            var query = stat.Include(u => u.AppUserTeams)
                                      .ThenInclude(u => u.Team)
                                      .ProjectTo<UserWithTeamDTO>(_mapper.ConfigurationProvider).AsQueryable();

            return await PaginationService.GetPagination<UserWithTeamDTO>(query, paginationParams.number, paginationParams.size);
        }

        public void RetrieveUser(AppUser user)
        {
            user.isActive = true;
            user.DeletedAt = null;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser user, int id)
        {
            var _user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (_user != null)
            {
                _user.Nickname = user.Nickname;
                _user.Fullname = user.Fullname;
                _user.Position = user.Position;
                _user.Role = user.Role;
                _user.isActive = user.isActive;
            }

            return _user;
        }

    }
}