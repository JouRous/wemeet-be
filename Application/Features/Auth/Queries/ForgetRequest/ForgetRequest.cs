using MediatR;

namespace Application.Features.Queries
{
    public class ForgetPasswordRequest : IRequest
    {
        public string email { get; set; }
        public string domain { get; set; }
    }
}