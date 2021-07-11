using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllBuildingQueryHandler : IRequestHandler<GetAllBuildingQuery, Pagination<BuildingDTO>>
    {
        private readonly IBuildingRepository _buildingRepo;

        public GetAllBuildingQueryHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepo = buildingRepository;
        }

        public async Task<Pagination<BuildingDTO>> Handle(GetAllBuildingQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _buildingRepo.GetAllAsync(request.query);

            return buildings;
        }
    }
}