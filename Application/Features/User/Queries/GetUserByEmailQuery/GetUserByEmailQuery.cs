using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetUserByEmailQuery : IRequest<UserDTO>
    {
        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}