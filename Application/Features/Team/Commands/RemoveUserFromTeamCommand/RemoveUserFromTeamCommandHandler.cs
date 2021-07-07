using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class RemoveUserFromTeamCommandHandler : IRequestHandler<RemoveUserFromTeamCommand>
    {
        private readonly ITeamRepository _teamRepo;

        public RemoveUserFromTeamCommandHandler(ITeamRepository teamRepository)
        {
            _teamRepo = teamRepository;
        }

        public async Task<Unit> Handle(RemoveUserFromTeamCommand request, CancellationToken cancellationToken)
        {
            await _teamRepo.RemoveUserFromTeam(request.teamId, request.userIds);
            return Unit.Value;
        }
    }
}