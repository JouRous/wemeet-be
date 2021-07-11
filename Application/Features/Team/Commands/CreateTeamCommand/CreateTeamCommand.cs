using System;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateTeamCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatorId { get; set; }
    }
}