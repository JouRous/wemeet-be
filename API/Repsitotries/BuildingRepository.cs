using API.Interfaces;
using System.Collections.Generic;
using System;
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

		public async Task<Pagination<BuildingDTO>> GetAllByPaginationAsync(
			PaginationParams paginationParams, string filter, string sort)
		{
			var stat = _context.Buildings.Where(t => t.Name.Contains(filter))
			.ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider);

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
			return await PaginationService.GetPagination<BuildingDTO>(query, paginationParams.pageNumber, paginationParams.pageSize);
		}

		public void ModifyOne(BuildingDTO building)
		{
			var entity = _context.Buildings.Find(building.Id);

			if (entity != null)
			{
				entity.UpdatedAt = DateTime.Now;
				entity.Address = building.Address == null ? entity.Address : building.Address;
				entity.Name = building.Name == null ? entity.Name : building.Name;
			}

			_context.Buildings.Update(entity);

		}

		public void DeletingOne(int Id)
		{
			try
			{
				var entity = _context.Buildings.Find(Id);
				if (entity == null) throw new Exception("entity is not exist!");
				_context.Buildings.Remove(entity);
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public async Task<BuildingDTO> GetOneAsync(int Id)
		{
			var res = await _context.Buildings.Where(building => building.Id == Id)
																 .ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider)
																 .SingleOrDefaultAsync();
			return res == null ? default : res;
		}

	}
}