using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDTO>
    {
        private readonly IUserRepository _userRepo;

        public GetUserByEmailQueryHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDTO> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userRepo.GetUserByEmailAsync(request.Email);
        }
    }
}