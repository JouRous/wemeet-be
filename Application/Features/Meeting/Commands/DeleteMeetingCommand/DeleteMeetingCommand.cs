using System;
using MediatR;

namespace Application.Features.Commands
{
    public class DeleteMeetingCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public DeleteMeetingCommand(Guid Id)
        {
            this.Id = Id;
        }
    }
}