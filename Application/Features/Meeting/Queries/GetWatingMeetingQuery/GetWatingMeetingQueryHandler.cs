using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Features.Queries
{
    public class GetWatingMeetingQueryHandler : IRequestHandler<GetWatingMeetingQuery, Pagination<MeetingBaseDTO>>
    {
        private readonly IMeetingRepo _meetingRepo;

        public GetWatingMeetingQueryHandler(IMeetingRepo meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public async Task<Pagination<MeetingBaseDTO>> Handle(GetWatingMeetingQuery request, CancellationToken cancellationToken)
        {
            var meetings = await _meetingRepo.GetWaitingMeetingAsync(request.query);
            return meetings;
        }
    }
}