using System.Collections.Generic;
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
using Infrastructure.Data;
using System;

namespace Infrastructure.Repositories
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

        public async Task AddTeamAsync(Team team)
        {
            team.AppUserTeams = new List<AppUserTeam>();
            _context.Teams.Add(team);

            await _context.SaveChangesAsync();
        }

        public async Task<Pagination<TeamWithUserDTO>> GetAllAsync(Query<FilterTeamModel> teamQuery)
        {
            var _filter = teamQuery.filter;
            var paginationParams = teamQuery.paginationParams;
            var sort = teamQuery.sort;

            var stat = _context.Teams
                .Where(t => t.Name.Contains(_filter.Name))
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


        public async Task<TeamWithUserDTO> GetTeamAsync(Guid teamId)
        {
            return await _context.Teams.ProjectTo<TeamWithUserDTO>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(t => t.Id == teamId);
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

        public async Task AddUserToTeamAsync(Guid teamId, ICollection<int> userIds)
        {
            var team = await _context.Teams.FindAsync(teamId);

            _context.AppUserTeams.RemoveRange(
                _context.AppUserTeams.Where(ut => ut.TeamId == teamId)
            );

            foreach (var userId in userIds)
            {
                team.AppUserTeams.Add(new AppUserTeam
                {
                    TeamId = team.Id,
                    AppUserId = userId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromTeam(Guid teamId, ICollection<int> userIds)
        {
            var team = await _context.Teams.Include(t => t.AppUserTeams).FirstOrDefaultAsync(t => t.Id == teamId);

            _context.AppUserTeams.RemoveRange(
                _context.AppUserTeams.Where(ut => ut.TeamId == teamId && userIds.Any(id => ut.AppUserId == id))
            );

            await _context.SaveChangesAsync();
        }
    }
}
