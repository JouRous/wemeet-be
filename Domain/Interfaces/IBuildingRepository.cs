using Domain.Entities;
using Domain.Types;
using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Domain.Interfaces
{
    public interface IBuildingRepository
    {
        Task CreateAsync(Building building);
        Task UpdateAsync(Building building);
        Task DeleteAsync(Building building);
        Task<Pagination<BuildingDTO>> GetAllAsync(Query<BuildingFilterModel> query);
        Task<Building> GetOneAsync(Guid Id);
    }
}