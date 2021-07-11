using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;

        public GetProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var email = handler.ReadJwtToken(request.Token).Claims
                .Where(c => c.Type.Equals("email"))
                .Select(c => c.Value)
                .SingleOrDefault();

            return await _userRepository.GetUserByEmailAsync(email);
        }
    }
}