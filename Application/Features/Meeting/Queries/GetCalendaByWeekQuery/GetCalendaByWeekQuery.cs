using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Queries
{
    public class GetCalendarByWeekQuery : IRequest<IEnumerable<object>>
    {
        public GetCalendarByWeekQuery(Guid roomId, DateTime monday, string userId)
        {
            RoomId = roomId;
            Monday = monday;
            UserId = userId;
        }

        public Guid RoomId { get; set; }
        public DateTime Monday { get; set; }
        public string UserId { get; set; }
    }
}