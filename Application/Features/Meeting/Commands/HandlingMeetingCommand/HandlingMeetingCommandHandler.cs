using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class HandlingMeetingCommandHandler : IRequestHandler<HandlingMeetingCommand>
    {
        private readonly IMeetingRepo _meetingRepo;

        public HandlingMeetingCommandHandler(IMeetingRepo meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public async Task<Unit> Handle(HandlingMeetingCommand request, CancellationToken cancellationToken)
        {
            var meetingToProcess = await _meetingRepo.GetOneAsync(request.MeetingId);

            meetingToProcess.Status = request.Status;

            await _meetingRepo.Update(meetingToProcess);

            return Unit.Value;
        }
    }
}