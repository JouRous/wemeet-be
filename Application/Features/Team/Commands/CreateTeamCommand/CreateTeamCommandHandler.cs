using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.Exceptions;
using MediatR;
using Domain.Types;

namespace Application.Features.Commands
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid>
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public CreateTeamCommandHandler(ITeamRepository teamRepo, IUserRepository userRepository, IMapper mapper)
        {
            _teamRepo = teamRepo;
            _userRepo = userRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {

            var leader = await _userRepo.GetUserEntityAsync(request.CreatorId);

            if (leader == null)
            {
                throw new NotFoundException(nameof(leader), request.CreatorId);
            }

            if (!leader.Role.Equals(UserRoles.LEAD))
            {
                throw new ForbiddenException("Only leader can create team");
            }

            var teamEntity = _mapper.Map<Team>(request);
            teamEntity.LeaderId = leader.Id;

            await _teamRepo.AddTeamAsync(teamEntity);
            await _teamRepo.AddOneUSerToTeam(teamEntity.Id, leader.Id);

            return teamEntity.Id;
        }
    }
}