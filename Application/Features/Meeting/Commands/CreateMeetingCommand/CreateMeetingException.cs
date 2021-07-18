using System;
using System.Collections.Generic;
using Domain.DTO;

namespace Application.Features.Commands
{
    public class CreateMeetingException : ApplicationException
    {
        public CreateMeetingException(IEnumerable<MeetingBase> conflictMeetings)
        {
            this.conflictMeetings = conflictMeetings;
        }

        public IEnumerable<MeetingBase> conflictMeetings { get; set; }

    }
}