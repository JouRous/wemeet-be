using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdateMeetingCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid Room_Id { get; set; }
        public ICollection<Guid> Team_Ids { get; set; }
        public ICollection<int> users_in_meeting { get; set; }

        public UpdateMeetingCommand(Guid Id)
        {
            this.Id = Id;
        }

    }
}