using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Commands
{
    public class UpdateMeetingCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid Room_Id { get; set; }
        public ICollection<Guid> Team_Ids { get; set; } = new List<Guid>();
        public ICollection<Guid> users_in_meeting { get; set; } = new List<Guid>();
        public ICollection<Guid> Tag_Ids { get; set; } = new List<Guid>();
        public ICollection<IFormFile> Attachments { get; set; } = new List<IFormFile>();

        public UpdateMeetingCommand()
        { }
        public UpdateMeetingCommand(Guid Id)
        {
            this.Id = Id;
        }
    }
}