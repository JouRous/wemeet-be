using MediatR;

namespace Application.Features.Commands
{
    public class ChangePasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string password { get; set; }

        public ChangePasswordCommand(string Token)
        {
            this.Token = Token;
        }
    }
}