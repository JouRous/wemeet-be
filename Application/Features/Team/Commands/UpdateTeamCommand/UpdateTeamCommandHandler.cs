using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using Application.Exceptions;
using MediatR;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Commands
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Guid>
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IMapper _mapper;

        public UpdateTeamCommandHandler(ITeamRepository teamRepo, IMapper mapper)
        {
            _teamRepo = teamRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var teamToUpdate = await _teamRepo.GetTeamEntityAsync(request.Id);

            if (teamToUpdate == null)
            {
                throw new NotFoundException(nameof(teamToUpdate), request.Id);
            }

            _mapper.Map(request, teamToUpdate, typeof(UpdateTeamCommand), typeof(Team));
            await _teamRepo.UpdateTeamAsync(teamToUpdate);

            return teamToUpdate.Id;
        }
    }
}