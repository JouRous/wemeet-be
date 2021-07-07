using System;
using System.Collections.Generic;
using Domain.DTO;
using Domain.Types;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateMeetingCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid Room_Id { get; set; }
        public ICollection<Guid> Team_Ids { get; set; }
        public ICollection<int> users_in_meeting { get; set; }
    }
}