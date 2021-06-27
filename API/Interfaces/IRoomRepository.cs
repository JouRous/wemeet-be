using API.Entities;
using API.Types;
using API.DTO;
using API.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace API.Interfaces
{
	public interface IRoomRepository
	{
		void AddOne(Room info);
		Task<Pagination<RoomDTO>> GetAllByPaginationAsync(Dictionary<string, int> page,
																											 Dictionary<string, string> filter,
																											 string sort = "-created_at");
		Task<RoomDTO> GetOneAsync(int Id);
		int GetSizeOfEntity(Func<Room, bool> query);
		void DeletingOne(int Id);
		void UpdatingOne(RoomDTO room);
	}
}