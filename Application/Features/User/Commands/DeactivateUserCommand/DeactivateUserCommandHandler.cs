using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
    {
        private readonly IUserRepository _userRepo;

        public DeactivateUserCommandHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var userToDeactivate = await _userRepo.GetUserEntityAsync(request.Id);

            if (userToDeactivate == null)
            {
                throw new NotFoundException(nameof(userToDeactivate), request.Id);
            }

            await _userRepo.DeactivateUser(userToDeactivate);
            return Unit.Value;
        }
    }
}