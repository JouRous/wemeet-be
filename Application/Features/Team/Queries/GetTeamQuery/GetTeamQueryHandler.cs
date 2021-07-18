using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, TeamWithUserDTO>
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IMapper _mapper;

        public GetTeamQueryHandler(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepo = teamRepository;
            _mapper = mapper;
        }

        public async Task<TeamWithUserDTO> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            return await _teamRepo.GetTeamAsync(request.Id);
        }
    }
}