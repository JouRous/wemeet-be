using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserWithTeamUsersDTO>
    {
        private readonly IUserRepository _userRepo;

        public GetUserQueryHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserWithTeamUsersDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _userRepo.GetUserAsync(request.Id);
        }
    }
}