using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using Domain.Types;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Domain.Interfaces;
using Application.Services;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BuildingRepository(AppDbContext app, IMapper map)
        {
            _context = app;
            _mapper = map;
        }
        public void AddOne(Building buildingInfo)
        {
            _context.Buildings.Add(buildingInfo);

        }

        public async Task<Pagination<BuildingDTO>> GetAllByPaginationAsync(Dictionary<string, int> page,
                                                                           Dictionary<string, string> filter,
                                                                           string sort = "-created_at")
        {
            var filterSerializer = JsonConvert.SerializeObject(filter);
            var pageSerializer = JsonConvert.SerializeObject(page);
            var _filter = JsonConvert.DeserializeObject<FilterTeamModel>(filterSerializer);
            var paginationParams = JsonConvert.DeserializeObject<PaginationParams>(pageSerializer);

            var stat = _context.Buildings.Where(t => t.Name.Contains(_filter.Name))
            .ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider);

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
            return await PaginationService.GetPagination<BuildingDTO>(query, paginationParams.number, paginationParams.size);
        }

        public void ModifyOne(BuildingDTO building)
        {
            var entity = _context.Buildings.Find(building.Id);

            if (entity != null)
            {
                entity.UpdatedAt = DateTime.Now;
                entity.Address = building.Address == null ? entity.Address : building.Address;
                entity.Name = building.Name == null ? entity.Name : building.Name;
            }

            _context.Buildings.Update(entity);

        }

        public void DeletingOne(Guid Id)
        {
            try
            {
                var entity = _context.Buildings.Find(Id);
                if (entity == null) throw new Exception("entity is not exist!");
                _context.Buildings.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<BuildingDTO> GetOneAsync(Guid Id)
        {
            // var res = await _context.Buildings.Where(building => building.Id == Id)
            //     .ProjectTo<BuildingDTO>(_mapper.ConfigurationProvider)
            //     .SingleOrDefaultAsync();
            // return res == null ? default : res;
            throw new Exception();
        }

    }
}