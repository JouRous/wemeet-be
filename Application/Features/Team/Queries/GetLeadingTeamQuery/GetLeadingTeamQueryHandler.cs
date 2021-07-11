using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetLeadingTeamQueryHandler : IRequestHandler<GetLeadingTeamQuery, IEnumerable<TeamBaseDTO>>
    {
        private readonly ITeamRepository _teamRepo;

        public GetLeadingTeamQueryHandler(ITeamRepository teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public async Task<IEnumerable<TeamBaseDTO>> Handle(GetLeadingTeamQuery request, CancellationToken cancellationToken)
        {
            var teams = await _teamRepo.GetLeadingTeamAsync(request.LeaderId);
            return teams;
        }
    }
}