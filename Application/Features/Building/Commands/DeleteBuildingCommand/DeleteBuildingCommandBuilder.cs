using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class DeleteBuildingCommandHandler : IRequestHandler<DeleteBuildingCommand, Guid>
    {
        private readonly IBuildingRepository _buildingRepo;

        public DeleteBuildingCommandHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepo = buildingRepository;
        }

        public async Task<Guid> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var buildingToUpdate = await _buildingRepo.GetOneAsync(request.Id);

            await _buildingRepo.DeleteAsync(buildingToUpdate);

            return request.Id;
        }
    }
}