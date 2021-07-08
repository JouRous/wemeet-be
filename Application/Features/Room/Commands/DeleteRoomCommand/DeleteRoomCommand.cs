using System;
using MediatR;

namespace Application.Features.Commands
{
    public class DeleteRoomCommand : IRequest<Guid>
    {
        public DeleteRoomCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

}