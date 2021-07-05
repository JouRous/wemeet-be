using System;
using System.Collections.Generic;
using Domain.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Features.Meeting.Commands
{
    public class CreateMeetingCommand : IRequest
    {
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public string Note { get; set; } = null;
        public ICollection<int> Users_In_Meeting { get; set; } = new List<int>();
        public int Team_Id { get; set; }
        public int RoomId { get; set; }
        public StatusMeeting Status { get; set; } = StatusMeeting.Waiting;
        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;
        public PriorityMeeting Priority { get; set; } = PriorityMeeting.Normal;
        public string Target { get; set; } = null;
        public MethodMeeting Method { get; set; } = MethodMeeting.Offline;
        public MeetingDTO ConflictWith { get; set; } = null;
    }
}