using System;
using System.Collections.Generic;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetLeadingTeamQuery : IRequest<IEnumerable<TeamBaseDTO>>
    {
        public Guid LeaderId { get; set; }
        public GetLeadingTeamQuery(Guid leaderId)
        {
            LeaderId = leaderId;
        }
    }
}