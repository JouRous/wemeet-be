using System;
using MediatR;

namespace Application.Features.Commands
{
    public class DeleteBuildingCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public DeleteBuildingCommand(Guid Id)
        {
            this.Id = Id;
        }
    }
}