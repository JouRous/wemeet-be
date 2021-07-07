using System;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetTeamQuery : IRequest<TeamWithUserDTO>
    {
        public Guid Id { get; set; }

        public GetTeamQuery(Guid Id)
        {
            this.Id = Id;
        }
    }
}