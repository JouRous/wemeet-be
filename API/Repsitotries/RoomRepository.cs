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
      _context.Rooms.Add(roomInfo);
    }

    public async Task<Pagination<RoomDTO>> GetAllByPaginationAsync(PaginationParams pageQuery)
    {
      var query = _context.Rooms.ProjectTo<RoomDTO>(_mapper.ConfigurationProvider).AsQueryable();
      var result = await PaginationService
                .GetPagination<RoomDTO>(query, pageQuery.pageNumber, pageQuery.pageSize);
      return result;
    }

    public async Task<RoomDTO> GetOneAsync(string Id)
    {
      var result = await _context.Rooms.Where(room => room.Id == Id)
                  .ProjectTo<RoomDTO>(_mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync();
      return result;
    }

  }
}