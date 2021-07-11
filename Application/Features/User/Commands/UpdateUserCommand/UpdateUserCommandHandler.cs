using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using API.Extensions;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Features.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UpdateUserCommandHandler(IUserRepository userRepo, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userToUpdate = await _userRepo.GetUserEntityAsync(request.Id);

            if (userToUpdate == null)
            {
                throw new NotFoundException(nameof(userToUpdate), request.Id);
            }

            _mapper.Map(request, userToUpdate, typeof(UpdateUserCommand), typeof(AppUser));
            userToUpdate.isActive = request.is_active;

            if (request.Avatar != null)
            {
                var pathToUpload = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", "Avatars");
                var uploadPath = await FileHelper.UploadFile(request.Avatar, pathToUpload);

                userToUpdate.Avatar = $"uploads/avatar/{uploadPath}";
            }

            await _userRepo.UpdateUserAsync(userToUpdate);

            return request.Id;
        }
    }
}