using Domain.DTO;
using Domain.Models;
using Domain.Types;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllMeetingQuery : IRequest<Pagination<MeetingDTO>>
    {
        public Query<MeetingFilterModel> query { get; set; }

        public GetAllMeetingQuery(Query<MeetingFilterModel> query)
        {
            this.query = query;
        }
    }
}