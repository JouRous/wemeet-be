using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(
            IUserRepository userRepository,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserEntityByEmailAsync(request.email);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), "Not found");
            }

            if (!user.isActive)
            {
                throw new ApplicationException("User already deactivated");
            }

            var loginStatus = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.password,
                false
            );

            if (!loginStatus.Succeeded)
            {
                throw new ApplicationException("Email or password incorrect");
            }

            return new AuthModel
            {
                token = await _tokenService.CreateToken(user),
                User = _mapper.Map<UserDTO>(user)
            };
        }
    }
}