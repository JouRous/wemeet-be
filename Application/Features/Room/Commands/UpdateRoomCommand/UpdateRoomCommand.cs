using System;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdateRoomCommand : IRequest<Guid>
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Guid Building_Id { get; set; }
    }
}