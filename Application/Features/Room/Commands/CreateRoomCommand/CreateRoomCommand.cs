using System;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateRoomCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Guid Building_Id { get; set; }
    }
}