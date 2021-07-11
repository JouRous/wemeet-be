using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Guid>
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;

        public UpdateRoomCommandHandler(IRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var roomToUpdate = await _roomRepo.GetRoom(request.RoomId);

            _mapper.Map(request, roomToUpdate, typeof(UpdateRoomCommand), typeof(Room));

            await _roomRepo.UpdateAsync(roomToUpdate);

            return roomToUpdate.Id;
        }
    }
}