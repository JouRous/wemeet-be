using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class UpdateMeetingCommandHandler : IRequestHandler<UpdateMeetingCommand, Guid>
    {
        private readonly IMeetingRepo _meetingRepo;
        private readonly IMapper _mapper;

        public UpdateMeetingCommandHandler(IMeetingRepo meetingRepo, IMapper mapper)
        {
            _meetingRepo = meetingRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateMeetingCommand request, CancellationToken cancellationToken)
        {
            var meetingToUpdate = await _meetingRepo.GetMeetingEntity(request.Id);

            _mapper.Map(request, meetingToUpdate, typeof(UpdateMeetingCommand), typeof(Meeting));

            await _meetingRepo.Update(meetingToUpdate);

            await _meetingRepo.AddUserToMeetingAsync(meetingToUpdate.Id, request.users_in_meeting);
            await _meetingRepo.AddTagToMeeting(meetingToUpdate.Id, request.Tag_Ids);
            await _meetingRepo.AddTeams(meetingToUpdate.Id, request.Team_Ids);

            return meetingToUpdate.Id;
        }
    }
}