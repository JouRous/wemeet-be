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
		Task<Pagination<BuildingDTO>> GetAllByPaginationAsync(
			PaginationParams paginationParams, string filter = "", string sort = "created_at"
			);
		Task<BuildingDTO> GetOneAsync(int Id);
		void ModifyOne(BuildingDTO building);
		void DeletingOne(int id);
	}
}