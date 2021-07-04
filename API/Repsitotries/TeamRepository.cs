using System.Collections.Generic;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TeamRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddTeam(Team team)
        {
            team.AppUserTeams = new List<AppUserTeam>();
            _context.Teams.Add(team);
        }

        public async Task<Pagination<TeamWithUserDTO>> GetAllAsync(Query<FilterTeamModel> teamQuery)
        {
            var _filter = teamQuery.filter;
            var paginationParams = teamQuery.paginationParams;
            var sort = teamQuery.sort;

            var stat = _context.Teams.Where(t => t.Name.Contains(_filter.Name))
                                      .Include(t => t.AppUserTeams)
                                      .ThenInclude(t => t.User)
                                      .ProjectTo<TeamWithUserDTO>(_mapper.ConfigurationProvider);

            switch (sort)
            {
                case "created_at":
                    stat = stat.OrderBy(t => t.CreatedAt);
                    break;
                case "-created_at":
                    stat = stat.OrderByDescending(t => t.CreatedAt);
                    break;
            }
            var query = stat.AsQueryable();
            return await PaginationService.GetPagination<TeamWithUserDTO>(query, paginationParams.number, paginationParams.size);
        }


        public async Task<TeamWithUserDTO> GetTeamAsync(int teamId)
        {
            return await _context.Teams.Where(team => team.Id == teamId)
                                       .ProjectTo<TeamWithUserDTO>(_mapper.ConfigurationProvider)
                                       .SingleOrDefaultAsync();
        }

        public async Task UpdateTeamAsync(Team team)
        {
            var _team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == team.Id);

            if (_team != null)
            {
                _team.Name = team.Name;
                _team.Description = team.Description;
            }
        }

        public async Task AddUserToTeamAsync(int teamId, ICollection<int> userIds)
        {
            var team = await _context.Teams.Include(t => t.AppUserTeams).FirstOrDefaultAsync(t => t.Id == teamId);

            team.AppUserTeams.Clear();
            foreach (var userId in userIds)
            {
                if (userIds.Count > 0)
                {
                    team.AppUserTeams.Add(new AppUserTeam
                    {
                        AppUserId = userId,
                        TeamId = team.Id
                    });
                }
            }
        }

        public async Task RemoveUserFromTeam(int teamId, ICollection<int> userIds)
        {
            var team = await _context.Teams.Include(t => t.AppUserTeams).FirstOrDefaultAsync(t => t.Id == teamId);

            foreach (var userId in userIds)
            {
                if (userIds.Count > 0)
                {
                    var relationUserTeam = await _context.AppUserTeams
                                            .FirstOrDefaultAsync(x => x.TeamId == team.Id && x.AppUserId == userId);
                    _context.Remove(relationUserTeam);
                }
            }
        }
    }
}
