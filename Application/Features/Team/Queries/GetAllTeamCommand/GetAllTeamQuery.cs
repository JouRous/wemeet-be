using Domain.DTO;
using Domain.Models;
using Domain.Types;
using MediatR;

namespace Application.Features.Team.Queries
{
    public class GetAllTeamQuery : IRequest<Pagination<TeamWithUserDTO>>
    {
        public Query<FilterTeamModel> query { get; set; }

        public GetAllTeamQuery(Query<FilterTeamModel> query)
        {
            this.query = query;
        }
    }
}