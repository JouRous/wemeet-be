using System.Collections.Generic;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetLeadingTeamQuery : IRequest<IEnumerable<TeamBaseDTO>>
    {
        public int LeaderId { get; set; }
        public GetLeadingTeamQuery(int leaderId)
        {
            LeaderId = leaderId;
        }
    }
}