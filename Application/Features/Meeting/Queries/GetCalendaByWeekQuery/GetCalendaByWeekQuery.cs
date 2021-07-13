using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Queries
{
    public class GetCalendarByWeekQuery : IRequest<IEnumerable<object>>
    {
        public GetCalendarByWeekQuery(Guid roomId, DateTime monday)
        {
            RoomId = roomId;
            Monday = monday;
        }

        public Guid RoomId { get; set; }
        public DateTime Monday { get; set; }
    }
}