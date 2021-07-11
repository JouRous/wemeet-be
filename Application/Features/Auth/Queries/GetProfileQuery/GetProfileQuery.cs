using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetProfileQuery : IRequest<UserDTO>
    {
        public GetProfileQuery(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}