using System;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetUserQuery : IRequest<UserWithTeamUsersDTO>
    {
        public GetUserQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}