using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Commands
{
    public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Guid>
    {
        private readonly IMeetingRepo _meetingRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateMeetingCommandHandler(
            IMeetingRepo meetingRepo,
            IFileRepository fileRepository,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _meetingRepo = meetingRepo;
            _mapper = mapper;
            _hostEnvironment = webHostEnvironment;
            _fileRepo = fileRepository;
        }

        public async Task<Guid> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var meetingEntity = _mapper.Map<Meeting>(request);

            meetingEntity.RoomId = request.Room_Id;

            await _meetingRepo.AddOneAsync(meetingEntity);

            await _meetingRepo.AddUserToMeetingAsync(meetingEntity.Id, request.users_in_meeting);
            await _meetingRepo.AddTagToMeeting(meetingEntity.Id, request.Tag_Ids);
            await _meetingRepo.AddTeams(meetingEntity.Id, request.Team_Ids);

            var pathToUpload = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", "Files");

            foreach (var file in request.Attachments)
            {
                var result = await FileHelper.UploadFile(file, pathToUpload);
                var fileEntity = new FileEntity
                {
                    FileName = file.FileName,
                    FileUrl = $"uploads/files/{result}"
                };
                await _fileRepo.Create(fileEntity);
                await _meetingRepo.AddFileToMeeting(meetingEntity.Id, fileEntity.Id);
            }

            return meetingEntity.Id;
        }
    }
}