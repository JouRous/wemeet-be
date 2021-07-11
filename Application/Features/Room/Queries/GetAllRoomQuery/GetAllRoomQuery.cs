using Domain.DTO;
using Domain.Models;
using Domain.Types;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllRoomQuery : IRequest<Pagination<RoomDTO>>
    {
        public GetAllRoomQuery(Query<RoomFilterModel> query)
        {
            this.query = query;
        }

        public Query<RoomFilterModel> query { get; set; }
    }
}