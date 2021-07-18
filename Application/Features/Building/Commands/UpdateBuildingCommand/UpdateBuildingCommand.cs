using System;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdatebuildingCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}