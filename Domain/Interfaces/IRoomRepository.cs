using Domain.Entities;
using Domain.Types;
using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> GetRoom(Guid id);
        Task<Pagination<RoomDTO>> GetAllAsync(Query<RoomFilterModel> query);
        Task AddOneAsync(Room Room);
        int GetSizeOfEntity(Func<Room, bool> query);
        Task DeleteAsync(Room room);
        Task UpdateAsync(Room room);
    }
}