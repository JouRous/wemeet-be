using AutoMapper;
using Domain.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;
using System.Linq;
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.DTO;
using Domain.Types;
using AutoMapper.QueryableExtensions;
using Application.Services;

namespace Infrastructure.Repositories
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

        public int GetSizeOfEntity(Func<Room, bool> query)
        {
            var count = _context.Rooms.Where(query).Count();
            return count;
        }

        public async Task AddOneAsync(Room Room)
        {
            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Room> GetRoom(Guid id)
        {
            return await _context.Rooms
                            .Include(r => r.Building)
                            .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Pagination<RoomDTO>> GetAllAsync(Query<RoomFilterModel> roomQuery)
        {
            var _filter = roomQuery.filter;
            var paginationParams = roomQuery.paginationParams;
            var sort = roomQuery.sort;

            var stat = _context.Rooms
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
    }
}