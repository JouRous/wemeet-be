using System;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetMeetingQuery : IRequest<MeetingDTO>
    {
        public Guid Id { get; set; }

        public GetMeetingQuery(Guid Id)
        {
            this.Id = Id;
        }
    }
}