using Domain.Entities;
using API.Types;
using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IBuildingRepository
    {
        void AddOne(Building buildingInfo);
        Task<Pagination<BuildingDTO>> GetAllByPaginationAsync(Dictionary<string, int> page,
                                                                                                             Dictionary<string, string> filter,
                                                                                                             string sort = "-created_at");
        Task<BuildingDTO> GetOneAsync(int Id);
        void ModifyOne(BuildingDTO building);
        void DeletingOne(int id);
    }
}