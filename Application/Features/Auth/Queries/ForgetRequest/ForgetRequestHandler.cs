using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class ForgetRequestHandler : IRequestHandler<ForgetPasswordRequest>
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public ForgetRequestHandler(IUserRepository userRepo, IEmailService emailService, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserEntityByEmailAsync(request.email);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), request.email);
            }

            var resetPasswordToken = _tokenService.CreateResetPasswordToken(user.Email);

            var resetPasswordLink = $"{request.domain}?token={resetPasswordToken}";

            await _emailService.sendMailAsync(user.Email, "Forget password", $@"<a href=""http://{resetPasswordLink}"">http://{resetPasswordLink}</a>");

            return Unit.Value;
        }
    }
}