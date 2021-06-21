using API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Models;
using API.Services;
using API.Types;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repsitotries
{
  public class BuildingRepository : IBuildingRepository
  {
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public BuildingRepository(AppDbContext app, IMapper map)
    {
      _context = app;
      _mapper = map;
    }
    public void AddOne(Building buildingInfo)
    {
      _context.Buildings.Add(buildingInfo);
    }

    public async Task<Pagination<BuildingDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery)
    {
      var data = _context.Buildings.ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider).AsQueryable();
      return await PaginationService.GetPagination<BuildingDTO>(data, paginationQuery.pageNumber, paginationQuery.pageSize);
    }
    public async Task<BuildingDTO> GetOneAsync(string Id)
    {
      return await _context.Buildings.Where(building => building.Id == Id)
                                 .ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider)
                                 .SingleOrDefaultAsync();
    }

  }
}