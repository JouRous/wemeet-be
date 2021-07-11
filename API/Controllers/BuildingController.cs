using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using Application.Features.Commands;
using MediatR;
using Application.Features.Queries;

namespace API.Controllers
{
    public class BuildingController : BaseApiController
    {
        private NotificationService _notificationService = new NotificationService();
        private readonly IMediator _mediator;

        public BuildingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBuildings(
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort)
        {
            var buildingQuery = QueryBuilder<BuildingFilterModel>
                        .Build(page, filter, sort);

            var query = new GetAllBuildingQuery(buildingQuery);

            var result = await _mediator.Send(query);

            var paginationDTO = new PaginationDTO
            {
                CurrentPage = result.CurrentPage,
                PerPage = result.PerPage,
                Total = result.Total,
                Count = result.Count,
                TotalPages = result.TotalPages
            };

            var response = new ResponseWithPaginationBuilder<IEnumerable<BuildingDTO>>()
                                .AddData(result.Items)
                                .AddPagination(paginationDTO)
                                .Build();

            return Ok(response);
        }

        [HttpGet("{buildingId}")]
        public async Task<ActionResult> GetBuilding(Guid buildingId)
        {
            var query = new GetBuildingQuery(buildingId);
            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<BuildingDTO>()
                                .AddData(result)
                                .Build();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBuilding([FromBody] CreateBuildingCommand command)
        {
            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Building has been created")
                                .Build();

            return Ok(response);
        }

        [HttpPut]
        [Route("{buildingId}")]
        public async Task<ActionResult> UpdateBuilding([FromRoute] Guid buildingId, [FromBody] UpdatebuildingCommand command)
        {
            command.Id = buildingId;

            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Building has been updated")
                                .Build();

            return Ok(response);
        }

        [HttpDelete("{buildingId}")]
        public async Task<ActionResult> RemoveBuilding(Guid buildingId)
        {
            var command = new DeleteBuildingCommand(buildingId);

            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Building has been deleted")
                                .Build();

            return Ok(response);
        }


    }
}