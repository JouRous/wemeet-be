using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Features.Queries
{
    public class GetMeetingByTeamQueryHandler : IRequestHandler<GetMeetingByTeamQuery, Pagination<MeetingDTO>>
    {
        private readonly IMeetingRepo _meetingRepo;

        public GetMeetingByTeamQueryHandler(IMeetingRepo meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public async Task<Pagination<MeetingDTO>> Handle(GetMeetingByTeamQuery request, CancellationToken cancellationToken)
        {
            return await _meetingRepo.GetAllByTeamAsync(request.TeamId, request.MeetingQuery);
        }
    }
}