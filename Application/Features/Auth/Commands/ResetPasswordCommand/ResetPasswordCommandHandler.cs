using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var email = handler.ReadJwtToken(request.token).Claims
                .Where(c => c.Type.Equals("email"))
                .Select(c => c.Value)
                .SingleOrDefault();

            var user = await _userRepository.GetUserEntityByEmailAsync(email);

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, request.password);

            return Unit.Value;
        }
    }
}