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

		public async Task<Pagination<Building>> GetAllByPaginationAsync(PaginationParams paginationQuery)
		{
			var data = _context.Buildings.ProjectTo<Building>(_mapper.ConfigurationProvider).AsQueryable();
			return await PaginationService.GetPagination<Building>(data, paginationQuery.currentPage, paginationQuery.pageSize);
		}
		public async Task<Building> GetOneAsync(string Id)
		{
			return await _context.Buildings.Where(building => building.Id == Id)
																 .ProjectTo<Building>(_mapper.ConfigurationProvider)
																 .SingleOrDefaultAsync();
		}

	}
}