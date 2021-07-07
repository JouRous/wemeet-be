using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.Commands
{
    public class AddUserToTeamCommand : IRequest
    {
        public Guid teamId { get; set; }
        public ICollection<int> userIds { get; set; }

        public AddUserToTeamCommand(Guid teamId, ICollection<int> userIds)
        {
            this.teamId = teamId;
            this.userIds = userIds;
        }
    }
}