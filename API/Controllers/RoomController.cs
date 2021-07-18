using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Application.Services;
using Domain.Interfaces;
using Domain.Types;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Utils;
using System;
using MediatR;
using Application.Features.Commands;
using Application.Features.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class RoomController : BaseApiController
    {
        private readonly IMediator _mediator;
        private NotificationService _notificationService = new NotificationService();

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<RoomDTO>>>> GetAlls(
            [FromQuery] Dictionary<string, int> page,
            [FromQuery] Dictionary<string, string> filter,
            [FromQuery] Dictionary<string, string> sort)
        {
            var roomQuery = QueryBuilder<RoomFilterModel>
                                .Build(page, filter, sort);

            var query = new GetAllRoomQuery(roomQuery);
            var result = await _mediator.Send(query);

            var pagination = new PaginationDTO
            {
                Count = result.Count,
                CurrentPage = result.CurrentPage,
                PerPage = result.PerPage,
                Total = result.Total,
                TotalPages = result.TotalPages
            };

            var response = new ResponseWithPaginationBuilder<IEnumerable<RoomDTO>>()
                                .AddData(result.Items)
                                .AddPagination(pagination)
                                .Build();

            return Ok(response);
        }

        [HttpGet("{RoomId}")]
        public async Task<ActionResult> GetRoomInfo(Guid roomId)
        {
            var query = new GetRoomQuery(roomId);
            var result = await _mediator.Send(query);

            var response = new ResponseBuilder<RoomDTO>()
                                .AddData(result)
                                .Build();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoom([FromBody] CreateRoomCommand command)
        {
            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .Build();

            return Ok(response);

        }

        [HttpPut]
        [Route("{roomId}")]
        public async Task<ActionResult> EditInfoRoom(
            [FromRoute] Guid roomId,
            [FromBody] UpdateRoomCommand command)
        {
            command.RoomId = roomId;

            var result = await _mediator.Send(command);
            var response = new ResponseBuilder<Guid>()
                                .AddData(result)
                                .AddMessage("Room has been updated")
                                .Build();

            return Ok(response);
        }

        [HttpDelete("{roomId}")]
        public async Task<ActionResult> RemoveRoom(Guid roomId)
        {
            var command = new DeleteRoomCommand(roomId);
            var result = await _mediator.Send(command);

            var response = new ResponseBuilder<Guid>()
                            .AddData(result)
                            .AddMessage("Room has been deleted")
                            .Build();

            return Ok(response);
        }

    }
}