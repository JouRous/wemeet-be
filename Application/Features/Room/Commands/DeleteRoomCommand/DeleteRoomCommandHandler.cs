using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, Guid>
    {
        private readonly IRoomRepository _roomRepo;

        public DeleteRoomCommandHandler(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<Guid> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var roomToDelete = await _roomRepo.GetRoom(request.Id);

            await _roomRepo.DeleteAsync(roomToDelete);

            return request.Id;
        }
    }
}