using System;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateBuildingCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}