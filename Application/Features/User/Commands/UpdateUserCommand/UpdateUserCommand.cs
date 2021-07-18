using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Commands
{
    public class UpdateUserCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Nickname { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public bool is_active { get; set; }
        public IFormFile Avatar { get; set; }
    }
}