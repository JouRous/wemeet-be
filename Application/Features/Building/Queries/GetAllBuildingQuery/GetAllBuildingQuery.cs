using Domain.DTO;
using Domain.Models;
using Domain.Types;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllBuildingQuery : IRequest<Pagination<BuildingDTO>>
    {
        public Query<BuildingFilterModel> query;

        public GetAllBuildingQuery(Query<BuildingFilterModel> query)
        {
            this.query = query;
        }
    }
}