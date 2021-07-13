using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Queries
{
    public class GetCalendaByMonthQuery : IRequest<IEnumerable<object>>
    {
        public GetCalendaByMonthQuery(Guid roomId, DateTime firstDay)
        {
            RoomId = roomId;
            this.firstDay = firstDay;
        }

        public Guid RoomId { get; set; }
        public DateTime firstDay { get; set; }
    }
}