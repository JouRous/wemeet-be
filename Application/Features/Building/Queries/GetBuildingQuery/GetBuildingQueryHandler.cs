using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetBuildingQueryHandler : IRequestHandler<GetBuildingQuery, BuildingDTO>
    {
        private readonly IBuildingRepository _buildingRepo;
        private readonly IMapper _mapper;

        public GetBuildingQueryHandler(IBuildingRepository buildingRepo, IMapper mapper)
        {
            _buildingRepo = buildingRepo;
            _mapper = mapper;
        }

        public async Task<BuildingDTO> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
        {
            var building = await _buildingRepo.GetOneAsync(request.Id);

            return _mapper.Map<BuildingDTO>(building);
        }
    }
}