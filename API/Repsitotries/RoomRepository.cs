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

		public void AddOne(RoomDTO roomInfo)
		{
			var room = _mapper.Map<Room>(roomInfo);
			_context.Rooms.Add(room);
		}

		public async Task<Pagination<RoomDTO>> GetAllByPaginationAsync(PaginationParams pageQuery)
		{
			var query = _context.Rooms.ProjectTo<RoomDTO>(_mapper.ConfigurationProvider).AsQueryable();
			var result = await PaginationService
								.GetPagination<RoomDTO>(query, pageQuery.currentPage, pageQuery.pageSize);
			return result;
		}

		public async Task<RoomDTO> GetOneAsync(string Id)
		{
			var result = await _context.Rooms.Where(room => room.Id == Id)
									.ProjectTo<RoomDTO>(_mapper.ConfigurationProvider)
									.SingleOrDefaultAsync();
			return result;
		}

		public int GetSizeOfEntity(Func<Room, bool> query)
		{
			var count = _context.Rooms.Where(query).Count();
			return count;
		}

		public void UpdatingOne(RoomDTO data)
		{
			var entity = _mapper.Map<Room>(data);
			_context.Rooms.Update(entity);
			_context.SaveChanges();
		}

		public void DeletingOne(string Id)
		{
			var entity = _context.Rooms.Find(Id);
			_context.Rooms.Remove(entity);
			_context.SaveChanges();
		}

	}
}