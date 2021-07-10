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

        public async Task CreateAsync(Building building)
        {
            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Building building)
        {
            _context.Entry(building).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Building building)
        {
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
        }

        public async Task<Building> GetOneAsync(Guid Id)
        {
            return await _context.Buildings.FindAsync(Id);
        }

        public async Task<Pagination<BuildingDTO>> GetAllAsync(Query<BuildingFilterModel> buildingQuery)
        {
            var _filter = buildingQuery.filter;
            var paginationParams = buildingQuery.paginationParams;
            var sort = buildingQuery.sort;

            var stat = _context.Buildings
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
    }
}