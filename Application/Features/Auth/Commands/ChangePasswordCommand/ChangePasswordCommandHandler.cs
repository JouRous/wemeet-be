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
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepo;

        public ChangePasswordCommandHandler(UserManager<AppUser> userManager, IUserRepository userRepo)
        {
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var email = handler.ReadJwtToken(request.Token).Claims
                .Where(c => c.Type.Equals("email"))
                .Select(c => c.Value)
                .SingleOrDefault();

            var user = await _userRepo.GetUserEntityByEmailAsync(email);

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, request.password);

            user.isFirstLogin = false;
            await _userManager.UpdateAsync(user);

            return Unit.Value;
        }
    }
}