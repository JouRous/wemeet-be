using System;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdateTeamCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}