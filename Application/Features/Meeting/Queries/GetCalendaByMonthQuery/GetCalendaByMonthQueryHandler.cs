using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetCalendaByMonthQueryHandler : IRequestHandler<GetCalendaByMonthQuery, IEnumerable<object>>
    {
        private readonly IMeetingRepo _meetingRepo;
        Calendar cal = new GregorianCalendar();

        public GetCalendaByMonthQueryHandler(IMeetingRepo meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public async Task<IEnumerable<object>> Handle(GetCalendaByMonthQuery request, CancellationToken cancellationToken)
        {
            CalendarWeekRule rule = CalendarWeekRule.FirstDay;

            var meetings = await _meetingRepo.GetMeetingByRoomAndDate(request.RoomId, request.firstDay, 30);

            var result = meetings
                        .OrderBy(x => x.StartTime)
                        .Select(m => new
                        {
                            id = m.Id,
                            start = m.StartTime,
                            end = m.EndTime,
                            minute_of_day = m.StartTime.Hour * 60 + m.StartTime.Minute,
                            meeting_time = (int)(m.EndTime - m.StartTime).TotalMinutes,
                            name = m.Name
                        })
                        .GroupBy(m => m.start.Day)
                        .ToDictionary(group => group.Key, group => group.OrderBy(x => x.start))
                        .Select(x => new
                        {
                            day = x.Key,
                            schedule = x.Value
                        })
                        .GroupBy(m =>
                        {
                            return cal.GetWeekOfYear(m.schedule.First().start, rule, DayOfWeek.Sunday);
                        })
                        .ToDictionary(group => group.Key)
                        .Select(x => new
                        {
                            week = x.Key,
                            data = x.Value
                        });

            return result;
        }
    }
}