using System;
using Domain.DTO;
using Domain.Models;
using Domain.Types;
using MediatR;

namespace Application.Features.Queries
{
    public class GetMeetingByTeamQuery : IRequest<Pagination<MeetingDTO>>
    {
        public GetMeetingByTeamQuery(Guid teamId, Query<MeetingFilterModel> meetingQuery)
        {
            TeamId = teamId;
            MeetingQuery = meetingQuery;
        }

        public Guid TeamId { get; set; }
        public Query<MeetingFilterModel> MeetingQuery { get; set; }
        public string Role { get; set; }
    }
}