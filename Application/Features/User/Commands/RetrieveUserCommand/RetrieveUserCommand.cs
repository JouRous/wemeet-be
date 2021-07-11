using System;
using MediatR;

namespace Application.Features.Commands
{
    public class RetrieveUserCommand : IRequest
    {
        public RetrieveUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}