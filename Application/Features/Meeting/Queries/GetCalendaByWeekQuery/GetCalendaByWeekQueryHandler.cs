using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetCalendaByWeekQueryHandler : IRequestHandler<GetCalendarByWeekQuery, IEnumerable<object>>
    {
        private readonly IMeetingRepo _meetingRepo;

        public GetCalendaByWeekQueryHandler(IMeetingRepo meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public async Task<IEnumerable<object>> Handle(GetCalendarByWeekQuery request, CancellationToken cancellationToken)
        {
            var meetings = await _meetingRepo.GetMeetingByRoomAndDate(request.RoomId, request.Monday, 7);

            var result = new List<object>();

            for (int i = 1; i < 7; i++)
            {
                var item = new Dictionary<string, object>();
                var schedule = meetings.Where(m => (int)m.StartTime.DayOfWeek == i).Select(m => new
                {
                    start = m.StartTime,
                    end = m.EndTime,
                    minute_of_day = m.StartTime.Hour * 60 + m.StartTime.Minute,
                    meeting_time = (int)(m.EndTime - m.StartTime).TotalMinutes,
                    name = m.Name
                }).ToList();

                item.Add("day_of_week", i - 1);
                item.Add("schedule", schedule);

                result.Add(item);
            }

            var sundaySchedule = new Dictionary<string, object>();
            var sundayMeeting = meetings.Where(m => (int)m.StartTime.DayOfWeek == 0).Select(m => new
            {
                start = m.StartTime,
                end = m.EndTime,
                minute_of_day = m.StartTime.Hour * 60 + m.StartTime.Minute,
                meeting_time = (int)(m.EndTime - m.StartTime).TotalMinutes,
                name = m.Name
            }).ToList();

            sundaySchedule.Add("day_of_week", 6);
            sundaySchedule.Add("schedule", sundayMeeting);

            result.Add(sundaySchedule);

            return result;
        }
    }
}