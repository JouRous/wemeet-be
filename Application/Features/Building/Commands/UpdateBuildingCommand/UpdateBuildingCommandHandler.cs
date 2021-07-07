using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdatebuildingCommandHandler : IRequestHandler<UpdatebuildingCommand, Guid>
    {
        private readonly IBuildingRepository _buildingRepo;
        private readonly IMapper _mapper;

        public UpdatebuildingCommandHandler(IBuildingRepository buildingRepository, IMapper mapper)
        {
            _buildingRepo = buildingRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdatebuildingCommand request, CancellationToken cancellationToken)
        {
            var buildingToUpdate = await _buildingRepo.GetOneAsync(request.Id);

            _mapper.Map(request, buildingToUpdate, typeof(UpdatebuildingCommand), typeof(Building));
            await _buildingRepo.UpdateAsync(buildingToUpdate);

            return buildingToUpdate.Id;
        }
    }
}