using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Queries
{
    public class GetCalendaByMonthQuery : IRequest<IEnumerable<object>>
    {
        public GetCalendaByMonthQuery(Guid roomId, DateTime firstDay, string userId)
        {
            RoomId = roomId;
            this.firstDay = firstDay;
            this.userId = userId;
        }

        public Guid RoomId { get; set; }
        public DateTime firstDay { get; set; }
        public string userId { get; set; }
    }
}