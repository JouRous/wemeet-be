using API.Entities;
using API.Types;
using API.DTO;
using API.Models;
using System.Threading.Tasks;

namespace API.Interfaces
{
	public interface IBuildingRepository
	{
		void AddOne(Building buildingInfo);
		Task<Pagination<BuildingDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery);
		Task<BuildingDTO> GetOneAsync(string Id);
		void ModifyOne(BuildingDTO building);
		void DeletingOne(string id);
	}
}