using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomQuery, Pagination<RoomDTO>>
    {
        private readonly IRoomRepository _roomRepo;

        public GetAllRoomQueryHandler(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<Pagination<RoomDTO>> Handle(GetAllRoomQuery request, CancellationToken cancellationToken)
        {
            var result = await _roomRepo.GetAllAsync(request.query);

            return result;
        }
    }
}