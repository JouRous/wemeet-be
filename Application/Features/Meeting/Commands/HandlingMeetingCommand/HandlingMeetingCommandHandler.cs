using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class HandlingMeetingCommandHandler : IRequestHandler<HandlingMeetingCommand>
    {
        private readonly IMeetingRepo _meetingRepo;
        private readonly IEmailService _emailService;

        public HandlingMeetingCommandHandler(IMeetingRepo meetingRepo, IEmailService emailService)
        {
            _meetingRepo = meetingRepo;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(HandlingMeetingCommand request, CancellationToken cancellationToken)
        {
            var meetingToProcess = await _meetingRepo.GetMeetingEntity(request.MeetingId);

            if (meetingToProcess == null)
            {
                // throw new NotFoundException(nameof(meetingToProcess), request.MeetingId);
            }

            meetingToProcess.Status = request.Status;

            await _meetingRepo.Update(meetingToProcess);

            var users = await _meetingRepo.GetUserInMeeting(meetingToProcess.Id);

            if (meetingToProcess.Status == StatusMeeting.Accepted)
            {
                foreach (var user in users)
                {
                    await _emailService.sendMailAsync(
                        user.Email,
                        $"Bạn được thêm vào cuộc họp {meetingToProcess.Name}",
                        $"Bạn được thêm vào cuộc họp {meetingToProcess.Name}");
                }
            }

            return Unit.Value;
        }
    }
}