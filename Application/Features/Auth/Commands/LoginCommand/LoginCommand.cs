using Domain.DTO;
using Domain.Models;
using MediatR;

namespace Application.Features.Commands
{
    public class LoginCommand : IRequest<AuthModel>
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}