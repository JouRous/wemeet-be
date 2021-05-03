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
      _context.Teams.Add(team);
    }

    public async Task<Pagination<TeamDTO>> GetAllAsync(PaginationParams paginationParams)
    {
      var query = _context.Teams.ProjectTo<TeamDTO>(_mapper.ConfigurationProvider).AsQueryable();

      return await PaginationService.GetPagination<TeamDTO>(query, paginationParams.currentPage, paginationParams.pageSize);
    }


    public Task<TeamDTO> GetTeamAsync()
    {
      throw new System.NotImplementedException();
    }
  }
}