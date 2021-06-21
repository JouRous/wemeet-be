using API.Entities;
using API.Types;
using API.DTO;
using API.Models;
using System.Threading.Tasks;
using System;

namespace API.Interfaces
{
	public interface IRoomRepository
	{
		void AddOne(Room info);
		Task<Pagination<RoomDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery, string f, string s);
		Task<RoomDTO> GetOneAsync(int Id);
		int GetSizeOfEntity(Func<Room, bool> query);
		void DeletingOne(int Id);
		void UpdatingOne(RoomDTO room);
	}
}