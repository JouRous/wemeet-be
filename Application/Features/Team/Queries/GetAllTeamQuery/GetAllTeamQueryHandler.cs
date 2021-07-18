using System.Threading;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllTeamQueryHandler : IRequestHandler<GetAllTeamQuery, Pagination<TeamWithUserDTO>>
    {
        private readonly ITeamRepository _repository;

        public GetAllTeamQueryHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public Task<Pagination<TeamWithUserDTO>> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
        {
            var teams = _repository.GetAllAsync(request.query);

            return teams;
        }
    }
}