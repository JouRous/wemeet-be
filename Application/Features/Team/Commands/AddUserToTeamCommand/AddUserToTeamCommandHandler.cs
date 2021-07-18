using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class AddUserToTeamCommandHandler : IRequestHandler<AddUserToTeamCommand>
    {
        private readonly ITeamRepository _teamRepo;

        public AddUserToTeamCommandHandler(ITeamRepository teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public async Task<Unit> Handle(AddUserToTeamCommand request, CancellationToken cancellationToken)
        {
            await _teamRepo.AddUserToTeamAsync(request.teamId, request.userIds);
            return Unit.Value;
        }
    }
}