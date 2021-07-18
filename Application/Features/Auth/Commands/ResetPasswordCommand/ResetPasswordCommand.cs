using MediatR;

namespace Application.Features.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        public string token { get; set; }
        public string password { get; set; }
    }
}