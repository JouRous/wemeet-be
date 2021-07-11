using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands
{
    public class RetrieveUserCommandHandler : IRequestHandler<RetrieveUserCommand>
    {
        private readonly IUserRepository _userRepo;

        public RetrieveUserCommandHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Unit> Handle(RetrieveUserCommand request, CancellationToken cancellationToken)
        {
            var userToDeactivate = await _userRepo.GetUserEntityAsync(request.Id);

            if (userToDeactivate == null)
            {
                throw new NotFoundException(nameof(userToDeactivate), request.Id);
            }

            await _userRepo.RetrieveUser(userToDeactivate);
            return Unit.Value;
        }
    }
}