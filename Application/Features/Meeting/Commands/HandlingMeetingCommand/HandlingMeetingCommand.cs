using System;
using Domain.Enums;
using MediatR;

namespace Application.Features.Commands
{
    public class HandlingMeetingCommand : IRequest
    {
        public HandlingMeetingCommand(Guid meetingId, StatusMeeting status)
        {
            MeetingId = meetingId;
            Status = status;
        }

        public Guid MeetingId { get; set; }
        public StatusMeeting Status { get; set; }
    }
}