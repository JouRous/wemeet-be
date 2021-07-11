using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class DeleteMeetingCommandHandler : IRequestHandler<DeleteMeetingCommand, Guid>
    {
        private readonly IMeetingRepo _meetingRepo;

        public DeleteMeetingCommandHandler(IMeetingRepo meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public async Task<Guid> Handle(DeleteMeetingCommand request, CancellationToken cancellationToken)
        {
            var meetingToDelete = await _meetingRepo.GetMeetingEntity(request.Id);
            await _meetingRepo.DeleteOneAsync(meetingToDelete);

            return request.Id;
        }
    }
}