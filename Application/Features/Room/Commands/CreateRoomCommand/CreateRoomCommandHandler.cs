using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler(IRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var roomEntity = _mapper.Map<Room>(request);
            roomEntity.BuildingId = request.Building_Id;

            await _roomRepo.AddOneAsync(roomEntity);

            return roomEntity.Id;
        }
    }
}