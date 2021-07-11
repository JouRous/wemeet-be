using System;
using Domain.DTO;
using MediatR;

namespace Application.Features.Queries
{
    public class GetBuildingQuery : IRequest<BuildingDTO>
    {
        public GetBuildingQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}