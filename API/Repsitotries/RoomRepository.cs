using AutoMapper;
using API.Data;
using API.Types;
using API.Interfaces;
using API.Entities;
using API.Models;
using API.DTO;
using API.Services;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.Repsitotries
{
	public class RoomRepository : IRoomRepository
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		public RoomRepository(AppDbContext app, IMapper map)
		{
			_context = app;
			_mapper = map;
		}

		public void AddOne(Room roomInfo)
		{
			var e = _context.Buildings.Find(roomInfo.BuildingId);

			if (e == null) throw new Exception("BuildingId not matching !");

			_context.Rooms.Add(roomInfo);
		}

		public async Task<Pagination<RoomDTO>> GetAllByPaginationAsync(Dictionary<string, int> page,
																											 Dictionary<string, string> filter,
																											 string sort = "-created_at")
		{
			var filterSerializer = JsonConvert.SerializeObject(filter);
			var pageSerializer = JsonConvert.SerializeObject(page);
			var _filter = JsonConvert.DeserializeObject<FilterTeamModel>(filterSerializer);
			var paginationParams = JsonConvert.DeserializeObject<PaginationParams>(pageSerializer);

			var stat = _context.Rooms.Where(t => t.Name.Contains(_filter.Name))
			.ProjectTo<RoomDTO>(_mapper.ConfigurationProvider);

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
			return await PaginationService.GetPagination<RoomDTO>(query, paginationParams.number, paginationParams.size);
		}

		public async Task<RoomDTO> GetOneAsync(int Id)
		{
			var result = await _context.Rooms.Where(room => room.Id == Id)
									.ProjectTo<RoomDTO>(_mapper.ConfigurationProvider)
									.SingleOrDefaultAsync();
			result.Building = _context.Buildings.Where(o => o.Id == result.Building.Id).ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider).FirstOrDefault();
			return result;
		}

		public int GetSizeOfEntity(Func<Room, bool> query)
		{
			var count = _context.Rooms.Where(query).Count();
			return count;
		}

		public void UpdatingOne(RoomDTO data)
		{
			var entity = _context.Rooms.Find(data.Id);
			if (data != null)
			{
				if (data.Building.Id > 0)
				{
					var e = _context.Buildings.Find(data.Building.Id);
					entity.BuildingId = e.Id;
				}
				entity.Capacity = data.Capacity == 0 ? entity.Capacity : data.Capacity;
				entity.Name = data.Name == null ? entity.Name : data.Name;
				entity.UpdatedAt = DateTime.Now;
			}

			_context.Rooms.Update(entity);

		}

		public void DeletingOne(int Id)
		{
			var entity = _context.Rooms.Find(Id);
			_context.Rooms.Remove(entity);

		}

	}
}