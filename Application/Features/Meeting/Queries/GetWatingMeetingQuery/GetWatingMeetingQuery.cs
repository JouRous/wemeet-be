using System;
using Domain.DTO;
using Domain.Models;
using Domain.Types;
using MediatR;

namespace Application.Features.Queries
{
    public class GetWatingMeetingQuery : IRequest<Pagination<MeetingBaseDTO>>
    {
        public Query<MeetingFilterModel> query;

        public GetWatingMeetingQuery(Query<MeetingFilterModel> query)
        {
            this.query = query;
        }
    }
}