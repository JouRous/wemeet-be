using System;
using MediatR;

namespace Application.Features.Commands
{
    public class DeactivateUserCommand : IRequest
    {
        public DeactivateUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}