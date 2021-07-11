using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using API.Extensions;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateUserCommandHandler(
            IUserRepository userRepo,
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IMapper mapper,
            IWebHostEnvironment hostEnvironment)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserEntityByEmailAsync(request.Email);

            if (user != null)
            {
                throw new ApplicationException("User already exist");
            }

            var pathToUpload = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", "Avatars");
            var uploadPath = await FileHelper.UploadFile(request.Avatar, pathToUpload);

            var userEntity = _mapper.Map<AppUser>(request);
            userEntity.UserName = userEntity.Email;
            userEntity.UnsignedName = StringHelper.RemoveAccentedString(userEntity.Fullname);
            userEntity.Avatar = $"uploads/avatar/{uploadPath}";

            var randomPassword = StringHelper.RandomString(9);
            var createStatus = await _userManager.CreateAsync(userEntity, randomPassword);

            await _emailService.sendMailAsync(userEntity.Email, "Dang ky thanh cong.", $"Mat khau la {randomPassword}");

            return userEntity.Id;
        }
    }
}