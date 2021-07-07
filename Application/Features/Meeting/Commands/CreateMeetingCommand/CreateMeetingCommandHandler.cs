using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Guid>
    {
        private readonly IMeetingRepo _meetingRepo;
        private readonly IMapper _mapper;

        public CreateMeetingCommandHandler(IMeetingRepo meetingRepo, IMapper mapper)
        {
            _meetingRepo = meetingRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var meetingEntity = _mapper.Map<Meeting>(request);

            meetingEntity.RoomId = request.Room_Id;

            await _meetingRepo.AddOneAsync(meetingEntity);

            await _meetingRepo.AddUserToMeetingAsync(meetingEntity.Id, request.users_in_meeting);

            return meetingEntity.Id;
        }
    }
}