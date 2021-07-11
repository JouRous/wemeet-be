using System;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetRoomQuery : IRequest<RoomDTO>
    {
        public GetRoomQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}