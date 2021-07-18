using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateBuildingCommandHandler : IRequestHandler<CreateBuildingCommand, Guid>
    {
        private readonly IBuildingRepository _buildingRepo;
        private readonly IMapper _mapper;

        public CreateBuildingCommandHandler(IBuildingRepository buildingRepository, IMapper mapper)
        {
            _buildingRepo = buildingRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            var buildingEntity = _mapper.Map<Building>(request);

            await _buildingRepo.CreateAsync(buildingEntity);

            return buildingEntity.Id;
        }
    }
}