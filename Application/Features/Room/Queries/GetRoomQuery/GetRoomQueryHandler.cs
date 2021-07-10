using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, RoomDTO>
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;

        public GetRoomQueryHandler(IRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }

        public async Task<RoomDTO> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetRoom(request.Id);
            return _mapper.Map<RoomDTO>(room);
        }
    }
}