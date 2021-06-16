using API.Entities;
using API.Types;
using API.DTO;
using API.Models;
using System.Threading.Tasks;

namespace API.Interfaces
{
	public interface IRoomRepository
	{
		void AddOne(Room info);
		Task<Pagination<RoomDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery);
		Task<RoomDTO> GetOneAsync(string Id);
	}
}