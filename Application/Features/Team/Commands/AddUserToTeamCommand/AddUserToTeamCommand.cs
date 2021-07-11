using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Commands
{
    public class AddUserToTeamCommand : IRequest
    {
        public Guid teamId { get; set; }
        public ICollection<Guid> userIds { get; set; }

        public AddUserToTeamCommand(Guid teamId, ICollection<Guid> userIds)
        {
            this.teamId = teamId;
            this.userIds = userIds;
        }
    }
}